using Application.Common;
using Application.Common.Errors;
using Application.Common.Mapping;
using Application.Pedidos.Dtos;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Pedidos.Commands.ActualizarPedido;

public sealed class ActualizarPedidoHandler(
    IPedidoRepository pedidoRepository,
    IPlatoRepository platoRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ActualizarPedidoCommand, Result<PedidoDto>>
{
    public async Task<Result<PedidoDto>> HandleAsync(
        ActualizarPedidoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var pedido = await pedidoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (pedido is null)
        {
            return Result.Failure<PedidoDto>(ApplicationErrors.NoEncontrado("Pedido", command.Id));
        }

        if (command.Estado.HasValue)
        {
            var estadoResult = pedido.ActualizarEstado(command.Estado.Value);
            if (estadoResult.IsFailure)
            {
                return Result.Failure<PedidoDto>(estadoResult.Error);
            }
        }

        if (command.Lineas is { Count: > 0 })
        {
            var platoIds = command.Lineas.Select(l => l.PlatoId).Distinct().ToList();
            var platos = await platoRepository.ObtenerPorIdsAsync(platoIds, cancellationToken);

            var solicitudes = command
                .Lineas.Select(l => new SolicitudLineaPedido(l.PlatoId, l.Cantidad))
                .ToList();

            var lineasResult = pedido.ActualizarLineas(solicitudes, platos);
            if (lineasResult.IsFailure)
            {
                return Result.Failure<PedidoDto>(lineasResult.Error);
            }
        }

        await pedidoRepository.ActualizarAsync(pedido, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(pedido.ToDto());
    }
}
