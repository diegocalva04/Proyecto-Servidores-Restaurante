using Application.Clientes.Dtos;
using Application.Common;
using Application.Common.Mapping;
using Domain.Repositories;

namespace Application.Clientes.Queries.ObtenerClientes;

public sealed class ObtenerClientesHandler(IClienteRepository clienteRepository)
    : IQueryHandler<ObtenerClientesQuery, IReadOnlyList<ClienteDto>>
{
    public async Task<IReadOnlyList<ClienteDto>> HandleAsync(
        ObtenerClientesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var clientes = await clienteRepository.ObtenerTodosAsync(cancellationToken);
        return clientes.Select(c => c.ToDto()).ToList();
    }
}
