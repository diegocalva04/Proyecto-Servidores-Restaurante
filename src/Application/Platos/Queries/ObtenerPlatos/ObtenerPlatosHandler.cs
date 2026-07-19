using Application.Common;
using Application.Common.Mapping;
using Application.Platos.Dtos;
using Domain.Repositories;

namespace Application.Platos.Queries.ObtenerPlatos;

public sealed class ObtenerPlatosHandler(IPlatoRepository platoRepository)
    : IQueryHandler<ObtenerPlatosQuery, IReadOnlyList<PlatoDto>>
{
    public async Task<IReadOnlyList<PlatoDto>> HandleAsync(
        ObtenerPlatosQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var platos = await platoRepository.ObtenerTodosAsync(cancellationToken);
        return platos.Select(p => p.ToDto()).ToList();
    }
}
