using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// Contexto de EF Core para persistir agregados del dominio en PostgreSQL.
public sealed class RestauranteDbContext(DbContextOptions<RestauranteDbContext> options)
    : DbContext(options)
{
    public DbSet<Plato> Platos => Set<Plato>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestauranteDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
