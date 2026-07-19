using Application.Common;
using Application.Common.Mapping;
using Application.Platos.Dtos;
using Domain.Repositories;

namespace Application.Platos.Queries.ObtenerPlatosDisponibles;

/// Handler del caso de uso de lectura: consultar platos disponibles.
public sealed class ObtenerPlatosDisponiblesHandler(IPlatoRepository platoRepository)
    : IQueryHandler<ObtenerPlatosDisponiblesQuery, IReadOnlyList<PlatoDto>>
{
    public async Task<IReadOnlyList<PlatoDto>> HandleAsync(
        ObtenerPlatosDisponiblesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var platos = await platoRepository.ObtenerDisponiblesAsync(cancellationToken);
        return platos.Select(p => p.ToDto()).ToList();
    }
}
