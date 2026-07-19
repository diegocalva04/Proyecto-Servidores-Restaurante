using Microsoft.OpenApi.Models;

namespace Api.DependencyInjection;

/// Registra servicios de la capa de presentación (API): controllers, Swagger y configuración HTTP.
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Sistema de Gestión de Restaurante API",
                    Version = "v1",
                    Description = "Backend REST para gestión de platos, clientes y pedidos.",
                }
            );
        });

        return services;
    }
}
