using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// Implementación EF Core de <see cref="IClienteRepository"/>.
public sealed class ClienteRepository(RestauranteDbContext context) : IClienteRepository
{
    public async Task<Cliente?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => await context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default
    ) => await context.Clientes.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Clientes.AnyAsync(c => c.Id == id, cancellationToken);

    public async Task AgregarAsync(
        Cliente cliente,
        CancellationToken cancellationToken = default
    ) => await context.Clientes.AddAsync(cliente, cancellationToken);

    public Task ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        context.Clientes.Remove(cliente);
        return Task.CompletedTask;
    }
}
