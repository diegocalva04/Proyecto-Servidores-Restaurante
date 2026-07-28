using Api.Extensions;
using Application.Clientes.Commands.ActualizarCliente;
using Application.Clientes.Commands.CrearCliente;
using Application.Clientes.Commands.EliminarCliente;
using Application.Clientes.Queries.ObtenerClientePorId;
using Application.Clientes.Queries.ObtenerClientes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// Endpoints HTTP para gestión de clientes.
[ApiController]
[Route("api/clientes")]
public sealed class ClientesController : ControllerBase
{
    /// Caso de uso principal de escritura: registra un nuevo cliente.
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearClienteCommand command,
        [FromServices] CrearClienteHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToCreatedResult(cliente =>
            Url.Action(nameof(ObtenerPorId), new { id = cliente.Id })!
        );
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(
        [FromServices] ObtenerClientesHandler handler,
        CancellationToken cancellationToken
    )
    {
        var clientes = await handler.HandleAsync(new ObtenerClientesQuery(), cancellationToken);
        return Ok(clientes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(
        Guid id,
        [FromServices] ObtenerClientePorIdHandler handler,
        CancellationToken cancellationToken
    )
    {
        var cliente = await handler.HandleAsync(
            new ObtenerClientePorIdQuery(id),
            cancellationToken
        );
        return cliente is null ? ResultExtensions.ToNotFoundResult("Cliente", id) : Ok(cliente);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarClienteRequest request,
        [FromServices] ActualizarClienteHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = new ActualizarClienteCommand(
            id,
            request.Nombre,
            request.Apellido,
            request.Correo,
            request.Telefono
        );

        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToOkResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(
        Guid id,
        [FromServices] EliminarClienteHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new EliminarClienteCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record ActualizarClienteRequest(
    string Nombre,
    string Apellido,
    string Correo,
    string Telefono
);
