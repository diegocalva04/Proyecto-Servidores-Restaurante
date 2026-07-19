using Domain.Repositories;

namespace Infrastructure.Persistence;

/// Implementación de la unidad de trabajo sobre el DbContext.
public sealed class UnitOfWork(RestauranteDbContext context) : IUnitOfWork
{
    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
