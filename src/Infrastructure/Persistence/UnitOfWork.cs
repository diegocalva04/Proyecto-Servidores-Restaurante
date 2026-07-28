using Domain.Common;
using Domain.Repositories;

namespace Infrastructure.Persistence;

/// Implementación de la unidad de trabajo sobre el DbContext.
public sealed class UnitOfWork(
    RestauranteDbContext context,
    IDomainEventDispatcher domainEventDispatcher) : IUnitOfWork
{
    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        var entities = context.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count > 0)
            .ToList();

        var result = await context.SaveChangesAsync(cancellationToken);
        var events = entities.SelectMany(entity => entity.DomainEvents).ToList();
        await domainEventDispatcher.DispatchAsync(events, cancellationToken);
        entities.ForEach(entity => entity.ClearDomainEvents());
        return result;
    }
}
