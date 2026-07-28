using Domain.Common;
using Domain.Events;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.DomainEvents;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

/// Registra persistencia (EF Core, PostgreSQL), repositorios e implementaciones de infraestructura.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IPlatoRepository, PlatoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventHandler<ClienteRegistrado>, ClienteRegistradoHandler>();
        services.AddScoped<IDomainEventHandler<PedidoRegistrado>, PedidoRegistradoHandler>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
