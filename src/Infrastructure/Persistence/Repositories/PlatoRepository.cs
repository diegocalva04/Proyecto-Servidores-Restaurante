using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// Implementación EF Core de <see cref="IPlatoRepository"/>.
public sealed class PlatoRepository(RestauranteDbContext context) : IPlatoRepository
{
    public async Task<Plato?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => await context.Platos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Plato>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default
    ) => await context.Platos.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Plato>> ObtenerDisponiblesAsync(
        CancellationToken cancellationToken = default
    ) =>
        await context.Platos.AsNoTracking().Where(p => p.Disponible).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Plato>> ObtenerPorIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    )
    {
        var idList = ids.Distinct().ToList();
        return await context
            .Platos.Where(p => idList.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(Plato plato, CancellationToken cancellationToken = default) =>
        await context.Platos.AddAsync(plato, cancellationToken);

    public Task ActualizarAsync(Plato plato, CancellationToken cancellationToken = default)
    {
        context.Platos.Update(plato);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Plato plato, CancellationToken cancellationToken = default)
    {
        context.Platos.Remove(plato);
        return Task.CompletedTask;
    }
}
