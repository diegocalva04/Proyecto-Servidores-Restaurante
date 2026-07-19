using Application.Clientes.Commands.ActualizarCliente;
using Application.Clientes.Commands.CrearCliente;
using Application.Clientes.Commands.EliminarCliente;
using Application.Clientes.Queries.ObtenerClientePorId;
using Application.Clientes.Queries.ObtenerClientes;
using Application.Pedidos.Commands.ActualizarPedido;
using Application.Pedidos.Commands.EliminarPedido;
using Application.Pedidos.Commands.RegistrarPedido;
using Application.Pedidos.Queries.ObtenerPedidoPorId;
using Application.Pedidos.Queries.ObtenerPedidos;
using Application.Platos.Commands.ActualizarPlato;
using Application.Platos.Commands.CrearPlato;
using Application.Platos.Commands.EliminarPlato;
using Application.Platos.Queries.ObtenerPlatoPorId;
using Application.Platos.Queries.ObtenerPlatos;
using Application.Platos.Queries.ObtenerPlatosDisponibles;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

/// Registra casos de uso (commands, queries y handlers) de la capa de aplicación.
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Platos
        services.AddScoped<ObtenerPlatosDisponiblesHandler>();
        services.AddScoped<ObtenerPlatosHandler>();
        services.AddScoped<ObtenerPlatoPorIdHandler>();
        services.AddScoped<CrearPlatoHandler>();
        services.AddScoped<ActualizarPlatoHandler>();
        services.AddScoped<EliminarPlatoHandler>();

        // Clientes
        services.AddScoped<CrearClienteHandler>();
        services.AddScoped<ActualizarClienteHandler>();
        services.AddScoped<EliminarClienteHandler>();
        services.AddScoped<ObtenerClientesHandler>();
        services.AddScoped<ObtenerClientePorIdHandler>();

        // Pedidos
        services.AddScoped<RegistrarPedidoHandler>();
        services.AddScoped<ActualizarPedidoHandler>();
        services.AddScoped<EliminarPedidoHandler>();
        services.AddScoped<ObtenerPedidosHandler>();
        services.AddScoped<ObtenerPedidoPorIdHandler>();

        return services;
    }
}
