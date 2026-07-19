using Application.Clientes.Dtos;
using Application.Common;
using Application.Common.Mapping;
using Domain.Repositories;

namespace Application.Clientes.Queries.ObtenerClientePorId;

public sealed class ObtenerClientePorIdHandler(IClienteRepository clienteRepository)
    : IQueryHandler<ObtenerClientePorIdQuery, ClienteDto?>
{
    public async Task<ClienteDto?> HandleAsync(
        ObtenerClientePorIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var cliente = await clienteRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
        return cliente?.ToDto();
    }
}
