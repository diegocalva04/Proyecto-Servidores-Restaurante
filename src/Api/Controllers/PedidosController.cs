using Api.Extensions;
using Application.Pedidos.Commands.ActualizarPedido;
using Application.Pedidos.Commands.EliminarPedido;
using Application.Pedidos.Commands.RegistrarPedido;
using Application.Pedidos.Dtos;
using Application.Pedidos.Queries.ObtenerPedidoPorId;
using Application.Pedidos.Queries.ObtenerPedidos;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// Endpoints HTTP para gestión de pedidos.
[ApiController]
[Route("api/pedidos")]
public sealed class PedidosController : ControllerBase
{
    /// Caso de uso principal de decisión: registra un pedido aplicando reglas de dominio.
    [HttpPost]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarPedidoRequest request,
        [FromServices] RegistrarPedidoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = new RegistrarPedidoCommand(request.ClienteId, request.Lineas);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToCreatedResult(
            pedido => Url.Action(nameof(ObtenerPorId), new { id = pedido.Id })!);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(
        [FromServices] ObtenerPedidosHandler handler,
        CancellationToken cancellationToken
    )
    {
        var pedidos = await handler.HandleAsync(new ObtenerPedidosQuery(), cancellationToken);
        return Ok(pedidos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(
        Guid id,
        [FromServices] ObtenerPedidoPorIdHandler handler,
        CancellationToken cancellationToken
    )
    {
        var pedido = await handler.HandleAsync(new ObtenerPedidoPorIdQuery(id), cancellationToken);
        return pedido is null ? ResultExtensions.ToNotFoundResult("Pedido", id) : Ok(pedido);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarPedidoRequest request,
        [FromServices] ActualizarPedidoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = new ActualizarPedidoCommand(id, request.Estado, request.Lineas);
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToOkResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(
        Guid id,
        [FromServices] EliminarPedidoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new EliminarPedidoCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}

public sealed record RegistrarPedidoRequest(
    Guid ClienteId,
    IReadOnlyList<LineaPedidoRequest> Lineas
);

public sealed record ActualizarPedidoRequest(
    EstadoPedido? Estado,
    IReadOnlyList<LineaPedidoRequest>? Lineas
);
