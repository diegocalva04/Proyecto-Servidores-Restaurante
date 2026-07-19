using Application.Platos.Commands.ActualizarPlato;
using Application.Platos.Commands.CrearPlato;
using Application.Platos.Commands.EliminarPlato;
using Application.Platos.Queries.ObtenerPlatos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Common;
using FluentAssertions;
using NSubstitute;

namespace Tests.Application;

public class PlatoCommandHandlerTests
{
    [Fact]
    public async Task ActualizarPlatoHandler_HandleAsync_DebeActualizarPlato_CuandoElPlatoExiste()
    {
        var plato = Plato.Crear("Causa", "Papa con pollo", 42.50m, CategoriaPlato.Entrada).Value;
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        repository.ObtenerPorIdAsync(plato.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Plato?>(plato));
        unitOfWork.GuardarCambiosAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        var handler = new ActualizarPlatoHandler(repository, unitOfWork);
        var command = new ActualizarPlatoCommand(
            plato.Id,
            "Causa limeña",
            "Papa con pollo y limón",
            45m,
            CategoriaPlato.Entrada,
            true
        );

        var resultado = await handler.HandleAsync(command);

        resultado.IsSuccess.Should().BeTrue();
        resultado.Value!.Nombre.Should().Be("Causa limeña");
        resultado.Value.Precio.Should().Be(45m);

        await repository.Received(1).ActualizarAsync(plato, Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ActualizarPlatoHandler_HandleAsync_DebeRetornarError_CuandoElPlatoNoExiste()
    {
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        repository.ObtenerPorIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Plato?>(null));

        var handler = new ActualizarPlatoHandler(repository, unitOfWork);
        var command = new ActualizarPlatoCommand(
            Guid.NewGuid(),
            "Causa limeña",
            "Papa con pollo y limón",
            45m,
            CategoriaPlato.Entrada,
            true
        );

        var resultado = await handler.HandleAsync(command);

        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Code.Should().Contain("NoEncontrado");
        await repository.DidNotReceiveWithAnyArgs().ActualizarAsync(default!, default);
        await unitOfWork.DidNotReceiveWithAnyArgs().GuardarCambiosAsync(default);
    }

    [Fact]
    public async Task EliminarPlatoHandler_HandleAsync_DebeEliminarPlato_CuandoElPlatoExiste()
    {
        var plato = Plato.Crear("Chicha morada", "Bebida tradicional", 18m, CategoriaPlato.Bebida).Value;
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        repository.ObtenerPorIdAsync(plato.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Plato?>(plato));
        unitOfWork.GuardarCambiosAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        var handler = new EliminarPlatoHandler(repository, unitOfWork);
        var command = new EliminarPlatoCommand(plato.Id);

        var resultado = await handler.HandleAsync(command);

        resultado.IsSuccess.Should().BeTrue();
        await repository.Received(1).EliminarAsync(plato, Arg.Any<CancellationToken>());
        await unitOfWork.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EliminarPlatoHandler_HandleAsync_DebeRetornarError_CuandoElPlatoNoExiste()
    {
        var repository = Substitute.For<IPlatoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        repository.ObtenerPorIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Plato?>(null));

        var handler = new EliminarPlatoHandler(repository, unitOfWork);
        var command = new EliminarPlatoCommand(Guid.NewGuid());

        var resultado = await handler.HandleAsync(command);

        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Code.Should().Contain("NoEncontrado");
        await repository.DidNotReceiveWithAnyArgs().EliminarAsync(default!, default);
        await unitOfWork.DidNotReceiveWithAnyArgs().GuardarCambiosAsync(default);
    }
}
