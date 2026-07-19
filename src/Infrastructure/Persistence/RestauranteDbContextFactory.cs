using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

/// Fábrica de diseño para generar migraciones sin levantar la aplicación completa.
public sealed class RestauranteDbContextFactory : IDesignTimeDbContextFactory<RestauranteDbContext>
{
    public RestauranteDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Api"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RestauranteDbContext>();
        optionsBuilder.UseNpgsql(
            configuration.GetConnectionString("restaurantedb"),
            npgsql => npgsql.MigrationsAssembly(typeof(RestauranteDbContext).Assembly.FullName)
        );

        return new RestauranteDbContext(optionsBuilder.Options);
    }
}
