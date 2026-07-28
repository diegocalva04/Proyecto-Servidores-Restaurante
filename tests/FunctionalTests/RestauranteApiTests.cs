using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using FluentAssertions;

namespace FunctionalTests;

public sealed class RestauranteApiFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    public HttpClient? Client { get; private set; }

    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        _app = await builder.BuildAsync();
        await _app.StartAsync();
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("postgres")
            .WaitAsync(TimeSpan.FromMinutes(2));
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("api")
            .WaitAsync(TimeSpan.FromMinutes(2));
        Client = _app.CreateHttpClient("api");
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}

public class RestauranteApiTests(RestauranteApiFixture fixture)
    : IClassFixture<RestauranteApiFixture>
{
    private HttpClient Client => fixture.Client!;

    [Fact]
    public async Task FlujoCompleto_PersisteYActualizaPedido()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var plato = await PostAndRead("/api/platos", new
        {
            nombre = $"Plato-{suffix}", descripcion = "Funcional", precio = 12.50m,
            categoria = 2, disponible = true
        }, HttpStatusCode.Created);
        var platoId = plato.GetProperty("id").GetGuid();

        (await Client.GetAsync($"/api/platos/{platoId}")).StatusCode.Should().Be(HttpStatusCode.OK);

        var cliente = await PostAndRead("/api/clientes", new
        {
            nombre = "Ana", apellido = "Prueba",
            correo = $"{suffix}@example.com", telefono = "0999123456"
        }, HttpStatusCode.Created);
        var clienteId = cliente.GetProperty("id").GetGuid();

        (await Client.GetAsync($"/api/clientes/{clienteId}")).StatusCode.Should().Be(HttpStatusCode.OK);

        var pedido = await PostAndRead("/api/pedidos", new
        {
            clienteId, lineas = new[] { new { platoId, cantidad = 2 } }
        }, HttpStatusCode.Created);
        var pedidoId = pedido.GetProperty("id").GetGuid();
        pedido.GetProperty("lineas").GetArrayLength().Should().Be(1);
        pedido.GetProperty("lineas")[0].GetProperty("subtotal").GetDecimal().Should().Be(25m);
        pedido.GetProperty("total").GetDecimal().Should().Be(25m);

        var update = await Client.PutAsJsonAsync($"/api/pedidos/{pedidoId}", new
        {
            estado = 2, lineas = (object?)null
        });
        update.StatusCode.Should().Be(HttpStatusCode.OK);

        var actualizado = await Client.GetFromJsonAsync<JsonElement>($"/api/pedidos/{pedidoId}");
        actualizado.GetProperty("estado").GetString().Should().Be("EnPreparacion");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CrearPlato_PrecioNoPositivo_Retorna400(decimal precio)
    {
        var response = await Client.PostAsJsonAsync("/api/platos", new
        {
            nombre = "Inválido", descripcion = "", precio, categoria = 1, disponible = true
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, "Plato.PrecioInvalido");
    }

    [Theory]
    [InlineData("correo", "0999123456", "Cliente.CorreoInvalido")]
    [InlineData("ok@example.com", "12", "Cliente.TelefonoInvalido")]
    public async Task CrearCliente_DatosInvalidos_Retorna400(
        string correo, string telefono, string code)
    {
        var response = await Client.PostAsJsonAsync("/api/clientes", new
        {
            nombre = "Ana", apellido = "Prueba", correo, telefono
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, code);
    }

    [Fact]
    public async Task CrearPedido_SinLineas_Retorna400()
    {
        var (clienteId, _) = await CrearPrerequisitos();
        var response = await Client.PostAsJsonAsync("/api/pedidos", new
        {
            clienteId, lineas = Array.Empty<object>()
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, "Pedido.SinPlatos");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CrearPedido_CantidadInvalida_Retorna400(int cantidad)
    {
        var (clienteId, platoId) = await CrearPrerequisitos();
        var response = await Client.PostAsJsonAsync("/api/pedidos", new
        {
            clienteId, lineas = new[] { new { platoId, cantidad } }
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, "Pedido.CantidadInvalida");
    }

    [Fact]
    public async Task CrearPedido_ClienteInexistente_Retorna404()
    {
        var response = await Client.PostAsJsonAsync("/api/pedidos", new
        {
            clienteId = Guid.NewGuid(), lineas = Array.Empty<object>()
        });
        await AssertProblem(response, HttpStatusCode.NotFound, null);
    }

    [Fact]
    public async Task CrearPedido_PlatoInexistente_Retorna400()
    {
        var (clienteId, _) = await CrearPrerequisitos();
        var response = await Client.PostAsJsonAsync("/api/pedidos", new
        {
            clienteId, lineas = new[] { new { platoId = Guid.NewGuid(), cantidad = 1 } }
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, "Pedido.PlatoNoEncontrado");
    }

    [Fact]
    public async Task CrearPedido_PlatoNoDisponible_Retorna400()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var cliente = await PostAndRead("/api/clientes", new
        {
            nombre = "Ana", apellido = "Prueba", correo = $"{suffix}@example.com", telefono = "0999123456"
        }, HttpStatusCode.Created);
        var plato = await PostAndRead("/api/platos", new
        {
            nombre = $"NoDisponible-{suffix}", descripcion = "", precio = 5m, categoria = 1, disponible = false
        }, HttpStatusCode.Created);
        var response = await Client.PostAsJsonAsync("/api/pedidos", new
        {
            clienteId = cliente.GetProperty("id").GetGuid(),
            lineas = new[] { new { platoId = plato.GetProperty("id").GetGuid(), cantidad = 1 } }
        });
        await AssertProblem(response, HttpStatusCode.BadRequest, "Pedido.PlatoNoDisponible");
    }

    [Fact]
    public async Task ModificarPedidoEnEstadoFinal_Retorna409()
    {
        var (clienteId, platoId) = await CrearPrerequisitos();
        var pedido = await PostAndRead("/api/pedidos", new
        {
            clienteId, lineas = new[] { new { platoId, cantidad = 1 } }
        }, HttpStatusCode.Created);
        var pedidoId = pedido.GetProperty("id").GetGuid();
        (await Client.PutAsJsonAsync($"/api/pedidos/{pedidoId}", new
        {
            estado = 4, lineas = (object?)null
        })).StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await Client.PutAsJsonAsync($"/api/pedidos/{pedidoId}", new
        {
            estado = (int?)null, lineas = new[] { new { platoId, cantidad = 2 } }
        });
        await AssertProblem(response, HttpStatusCode.Conflict, "Pedido.EstadoFinal");
    }

    [Theory]
    [InlineData("/api/platos/")]
    [InlineData("/api/clientes/")]
    [InlineData("/api/pedidos/")]
    public async Task ConsultarRecursoInexistente_Retorna404(string route)
    {
        await AssertProblem(
            await Client.GetAsync(route + Guid.NewGuid()),
            HttpStatusCode.NotFound,
            null);
    }

    private async Task<(Guid clienteId, Guid platoId)> CrearPrerequisitos()
    {
        var suffix = Guid.NewGuid().ToString("N");
        var cliente = await PostAndRead("/api/clientes", new
        {
            nombre = "Ana", apellido = "Prueba", correo = $"{suffix}@example.com", telefono = "0999123456"
        }, HttpStatusCode.Created);
        var plato = await PostAndRead("/api/platos", new
        {
            nombre = $"Plato-{suffix}", descripcion = "", precio = 5m, categoria = 1, disponible = true
        }, HttpStatusCode.Created);
        return (cliente.GetProperty("id").GetGuid(), plato.GetProperty("id").GetGuid());
    }

    private async Task<JsonElement> PostAndRead(string url, object body, HttpStatusCode status)
    {
        var response = await Client.PostAsJsonAsync(url, body);
        response.StatusCode.Should().Be(status);
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    private static async Task AssertProblem(
        HttpResponseMessage response, HttpStatusCode status, string? code)
    {
        response.StatusCode.Should().Be(status);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();
        problem.GetProperty("status").GetInt32().Should().Be((int)status);
        problem.GetProperty("detail").GetString().Should().NotBeNullOrWhiteSpace();
        if (code is not null)
        {
            problem.GetProperty("code").GetString().Should().Be(code);
        }
    }
}
