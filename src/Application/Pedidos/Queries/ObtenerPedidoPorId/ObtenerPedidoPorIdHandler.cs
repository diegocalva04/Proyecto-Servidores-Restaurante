using Application.Common;
using Application.Common.Mapping;
using Application.Pedidos.Dtos;
using Domain.Repositories;

namespace Application.Pedidos.Queries.ObtenerPedidoPorId;

public sealed class ObtenerPedidoPorIdHandler(IPedidoRepository pedidoRepository)
    : IQueryHandler<ObtenerPedidoPorIdQuery, PedidoDto?>
{
    public async Task<PedidoDto?> HandleAsync(
        ObtenerPedidoPorIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var pedido = await pedidoRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
        return pedido?.ToDto();
    }
}
