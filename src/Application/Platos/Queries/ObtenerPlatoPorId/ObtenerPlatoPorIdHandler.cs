using Application.Common;
using Application.Common.Mapping;
using Application.Platos.Dtos;
using Domain.Repositories;

namespace Application.Platos.Queries.ObtenerPlatoPorId;

public sealed class ObtenerPlatoPorIdHandler(IPlatoRepository platoRepository)
    : IQueryHandler<ObtenerPlatoPorIdQuery, PlatoDto?>
{
    public async Task<PlatoDto?> HandleAsync(
        ObtenerPlatoPorIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var plato = await platoRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
        return plato?.ToDto();
    }
}
