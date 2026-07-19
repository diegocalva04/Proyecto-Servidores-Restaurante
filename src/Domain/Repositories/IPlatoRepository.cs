using Domain.Entities;

namespace Domain.Repositories;

/// Contrato de persistencia para la entidad <see cref="Plato"/>.
public interface IPlatoRepository
{
    Task<Plato?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Plato>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Plato>> ObtenerDisponiblesAsync(
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Plato>> ObtenerPorIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    );

    Task AgregarAsync(Plato plato, CancellationToken cancellationToken = default);

    Task ActualizarAsync(Plato plato, CancellationToken cancellationToken = default);

    Task EliminarAsync(Plato plato, CancellationToken cancellationToken = default);
}
