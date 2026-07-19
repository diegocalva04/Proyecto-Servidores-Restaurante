using Application.Common;
using Application.Common.Errors;
using Domain.Common;
using Domain.Repositories;

namespace Application.Pedidos.Commands.EliminarPedido;

public sealed class EliminarPedidoHandler(
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<EliminarPedidoCommand, Result>
{
    public async Task<Result> HandleAsync(
        EliminarPedidoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var pedido = await pedidoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (pedido is null)
        {
            return Result.Failure(ApplicationErrors.NoEncontrado("Pedido", command.Id));
        }

        await pedidoRepository.EliminarAsync(pedido, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success();
    }
}
