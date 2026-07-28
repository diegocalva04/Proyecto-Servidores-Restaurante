using Application.Pedidos.Commands.RegistrarPedido;
using Application.Pedidos.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using FluentAssertions;
using NSubstitute;

namespace Tests.Application;

public sealed class RegistrarPedidoHandlerTests
{
    [Fact]
    public async Task HandleAsync_ConClienteYPlatoDisponible_RegistrarPedidoCorrectamente()
    {
        var clienteRepository = Substitute.For<IClienteRepository>();
        var platoRepository = Substitute.For<IPlatoRepository>();
        var pedidoRepository = Substitute.For<IPedidoRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var clock = Substitute.For<IClock>();

        clock.UtcNow.Returns(new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero));
        clienteRepository.ExisteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var plato = Plato.Crear("Pasta", "Salsa roja", 8.5m, CategoriaPlato.Principal, true).Value;
        platoRepository.ObtenerPorIdsAsync(Arg.Any<IReadOnlyList<Guid>>(), Arg.Any<CancellationToken>())
            .Returns([plato]);

        var handler = new RegistrarPedidoHandler(
            clienteRepository,
            platoRepository,
            pedidoRepository,
            unitOfWork,
            clock
        );

        var result = await handler.HandleAsync(
            new RegistrarPedidoCommand(
                Guid.NewGuid(),
                [new LineaPedidoRequest(plato.Id, 2)]
            ),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Total.Should().Be(17m);
        await pedidoRepository.Received(1).AgregarAsync(Arg.Any<Pedido>(), Arg.Any<CancellationToken>());
    }
}
