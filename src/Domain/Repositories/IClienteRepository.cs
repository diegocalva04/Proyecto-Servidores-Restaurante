using Domain.Entities;

namespace Domain.Repositories;

/// Contrato de persistencia para la entidad <see cref="Cliente"/>.
public interface IClienteRepository
{
    Task<Cliente?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default);

    Task AgregarAsync(Cliente cliente, CancellationToken cancellationToken = default);

    Task ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);

    Task EliminarAsync(Cliente cliente, CancellationToken cancellationToken = default);
}
