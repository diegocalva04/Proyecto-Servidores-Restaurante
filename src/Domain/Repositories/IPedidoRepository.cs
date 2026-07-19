using Domain.Entities;

namespace Domain.Repositories;

/// Contrato de persistencia para el agregado <see cref="Pedido"/>.
public interface IPedidoRepository
{
    Task<Pedido?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Pedido>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task AgregarAsync(Pedido pedido, CancellationToken cancellationToken = default);

    Task ActualizarAsync(Pedido pedido, CancellationToken cancellationToken = default);

    Task EliminarAsync(Pedido pedido, CancellationToken cancellationToken = default);
}
