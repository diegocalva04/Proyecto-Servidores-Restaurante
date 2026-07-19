using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// Implementación EF Core de <see cref="IPedidoRepository"/>.
public sealed class PedidoRepository(RestauranteDbContext context) : IPedidoRepository
{
    public async Task<Pedido?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        await context
            .Pedidos.Include(p => p.Lineas)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Pedido>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default
    ) => await context.Pedidos.AsNoTracking().Include(p => p.Lineas).ToListAsync(cancellationToken);

    public async Task AgregarAsync(Pedido pedido, CancellationToken cancellationToken = default) =>
        await context.Pedidos.AddAsync(pedido, cancellationToken);

    public Task ActualizarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        context.Pedidos.Update(pedido);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        context.Pedidos.Remove(pedido);
        return Task.CompletedTask;
    }
}
