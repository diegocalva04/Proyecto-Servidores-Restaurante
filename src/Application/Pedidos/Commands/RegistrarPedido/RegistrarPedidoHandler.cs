using Application.Common;
using Application.Common.Errors;
using Application.Common.Mapping;
using Application.Pedidos.Dtos;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Pedidos.Commands.RegistrarPedido;

/// Handler del caso de uso de decisión: orquesta búsqueda, validación de dominio y persistencia.
public sealed class RegistrarPedidoHandler(
    IClienteRepository clienteRepository,
    IPlatoRepository platoRepository,
    IPedidoRepository pedidoRepository,
    IUnitOfWork unitOfWork,
    IClock clock
) : ICommandHandler<RegistrarPedidoCommand, Result<PedidoDto>>
{
    public async Task<Result<PedidoDto>> HandleAsync(
        RegistrarPedidoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var clienteExiste = await clienteRepository.ExisteAsync(
            command.ClienteId,
            cancellationToken
        );
        if (!clienteExiste)
        {
            return Result.Failure<PedidoDto>(
                ApplicationErrors.NoEncontrado("Cliente", command.ClienteId)
            );
        }

        if (command.Lineas.Count == 0)
        {
            return Result.Failure<PedidoDto>(Domain.Errors.DomainErrors.Pedido.SinPlatos);
        }

        var platoIds = command.Lineas.Select(l => l.PlatoId).Distinct().ToList();
        var platos = await platoRepository.ObtenerPorIdsAsync(platoIds, cancellationToken);

        var solicitudes = command
            .Lineas.Select(l => new SolicitudLineaPedido(l.PlatoId, l.Cantidad))
            .ToList();

        var pedidoResult = Pedido.Crear(command.ClienteId, solicitudes, platos, clock.UtcNow);

        if (pedidoResult.IsFailure)
        {
            return Result.Failure<PedidoDto>(pedidoResult.Error);
        }

        await pedidoRepository.AgregarAsync(pedidoResult.Value, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(pedidoResult.Value.ToDto());
    }
}
