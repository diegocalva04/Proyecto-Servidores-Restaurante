using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using FluentAssertions;

namespace Tests.Domain;

public class PedidoTests
{
    [Fact]
    public void Crear_Valido_CalculaTotalesYGeneraEvento()
    {
        var plato = Plato.Crear("Seco", "Pollo", 10.50m, CategoriaPlato.Principal).Value;
        var result = CrearPedido(plato, 2);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.ClienteId.Should().NotBeEmpty();
        result.Value.Lineas.Single().Subtotal.Valor.Should().Be(21m);
        result.Value.Total.Valor.Should().Be(21m);
        result.Value.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<PedidoRegistrado>();
    }

    [Fact]
    public void Crear_ClienteVacio_Falla() =>
        Pedido.Crear(Guid.Empty, [], [], DateTimeOffset.UtcNow)
            .Error.Code.Should().Be("Pedido.ClienteRequerido");

    [Fact]
    public void Crear_SinLineas_Falla() =>
        Pedido.Crear(Guid.NewGuid(), [], [], DateTimeOffset.UtcNow)
            .Error.Code.Should().Be("Pedido.SinPlatos");

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Crear_CantidadInvalida_Falla(int cantidad)
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        CrearPedido(plato, cantidad).Error.Code.Should().Be("Pedido.CantidadInvalida");
    }

    [Fact]
    public void Crear_PlatoNoDisponible_Falla()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada, false).Value;
        CrearPedido(plato, 1).Error.Code.Should().Be("Pedido.PlatoNoDisponible");
    }

    [Fact]
    public void ActualizarLineas_RecalculaTotal()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        var pedido = CrearPedido(plato, 1).Value;
        pedido.ActualizarLineas([new(plato.Id, 3)], [plato]).IsSuccess.Should().BeTrue();
        pedido.Total.Valor.Should().Be(15m);
    }

    [Fact]
    public void CambiarEstado_RespetaTransiciones()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        var pedido = CrearPedido(plato, 1).Value;
        pedido.ActualizarEstado(EstadoPedido.EnPreparacion).IsSuccess.Should().BeTrue();
        pedido.ActualizarEstado(EstadoPedido.Entregado).IsSuccess.Should().BeTrue();
        pedido.Estado.Should().Be(EstadoPedido.Entregado);
    }

    [Fact]
    public void TransicionInvalida_Falla()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        CrearPedido(plato, 1).Value.ActualizarEstado(EstadoPedido.Entregado)
            .Error.Code.Should().Be("Pedido.EstadoInvalido");
    }

    [Fact]
    public void EstadoFinal_ImpideModificarLineas()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        var pedido = CrearPedido(plato, 1).Value;
        pedido.ActualizarEstado(EstadoPedido.Cancelado);
        pedido.ActualizarLineas([new(plato.Id, 2)], [plato])
            .Error.Code.Should().Be("Pedido.EstadoFinal");
    }

    [Fact]
    public void LimpiarEventos_EliminaEventos()
    {
        var plato = Plato.Crear("Sopa", "", 5m, CategoriaPlato.Entrada).Value;
        var pedido = CrearPedido(plato, 1).Value;
        pedido.ClearDomainEvents();
        pedido.DomainEvents.Should().BeEmpty();
    }

    private static global::Domain.Common.Result<Pedido> CrearPedido(Plato plato, int cantidad) =>
        Pedido.Crear(
            Guid.NewGuid(),
            [new SolicitudLineaPedido(plato.Id, cantidad)],
            [plato],
            DateTimeOffset.UtcNow);
}
