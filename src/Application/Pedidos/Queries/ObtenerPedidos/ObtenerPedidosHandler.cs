using Application.Common;
using Application.Common.Mapping;
using Application.Pedidos.Dtos;
using Domain.Repositories;

namespace Application.Pedidos.Queries.ObtenerPedidos;

public sealed class ObtenerPedidosHandler(IPedidoRepository pedidoRepository)
    : IQueryHandler<ObtenerPedidosQuery, IReadOnlyList<PedidoDto>>
{
    public async Task<IReadOnlyList<PedidoDto>> HandleAsync(
        ObtenerPedidosQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var pedidos = await pedidoRepository.ObtenerTodosAsync(cancellationToken);
        return pedidos.Select(p => p.ToDto()).ToList();
    }
}
