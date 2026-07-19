using Application.Platos.Commands.CrearPlato;
using Application.Platos.Queries.ObtenerPlatos;
using Application.Platos.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Common;
using FluentAssertions;
using NSubstitute;

namespace Tests.Application;

public class PlatoHandlerTests
{
    [Fact]
    public async Task CrearPlatoHandler_HandleAsync_DebeCrearPlato_CuandoElComandoEsValido()
    {
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.GuardarCambiosAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        var handler = new CrearPlatoHandler(repository, unitOfWork);
        var command = new CrearPlatoCommand("Lomo saltado", "Carne con papas", 79.50m, CategoriaPlato.Principal);

        var resultado = await handler.HandleAsync(command);

        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Nombre.Should().Be("Lomo saltado");
        resultado.Value.Precio.Should().Be(79.50m);
        resultado.Value.Categoria.Should().Be(CategoriaPlato.Principal.ToString());

        await repository.Received(1).AgregarAsync(Arg.Any<Plato>(), Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CrearPlatoHandler_HandleAsync_DebeRetornarFallo_CuandoElNombreEsInvalido()
    {
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var handler = new CrearPlatoHandler(repository, unitOfWork);
        var command = new CrearPlatoCommand(string.Empty, "Sin nombre", 45m, CategoriaPlato.Principal);

        var resultado = await handler.HandleAsync(command);

        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Code.Should().Be("Plato.NombreInvalido");

        await repository.DidNotReceiveWithAnyArgs().AgregarAsync(default!, default);
        await unitOfWork.DidNotReceiveWithAnyArgs().GuardarCambiosAsync(default);
    }

    [Fact]
    public async Task ObtenerPlatosHandler_HandleAsync_DebeRetornarDtos_CuandoHayPlatos()
    {
        var repository = Substitute.For<IPlatoRepository>();
        var platos = new List<Plato>
        {
            Plato.Crear("Ceviche", "Pescado marinado", 59.99m, CategoriaPlato.Entrada).Value,
            Plato.Crear("Churrasco", "Carne con papas", 85m, CategoriaPlato.Principal).Value
        };

        repository.ObtenerTodosAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IReadOnlyList<Plato>>(platos));

        var handler = new ObtenerPlatosHandler(repository);

        var resultado = await handler.HandleAsync(new ObtenerPlatosQuery());

        resultado.Should().HaveCount(2);
        resultado[0].Nombre.Should().Be("Ceviche");
        resultado[1].Nombre.Should().Be("Churrasco");
    }
}
