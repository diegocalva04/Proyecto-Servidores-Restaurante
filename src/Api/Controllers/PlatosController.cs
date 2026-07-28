using Api.Extensions;
using Application.Platos.Commands.ActualizarPlato;
using Application.Platos.Commands.CrearPlato;
using Application.Platos.Commands.EliminarPlato;
using Application.Platos.Queries.ObtenerPlatoPorId;
using Application.Platos.Queries.ObtenerPlatos;
using Application.Platos.Queries.ObtenerPlatosDisponibles;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// Endpoints HTTP para gestión de platos del menú.
[ApiController]
[Route("api/platos")]
public sealed class PlatosController : ControllerBase
{
    /// Caso de uso principal de lectura: consulta platos disponibles.
    [HttpGet("disponibles")]
    public async Task<IActionResult> ObtenerDisponibles(
        [FromServices] ObtenerPlatosDisponiblesHandler handler,
        CancellationToken cancellationToken
    )
    {
        var platos = await handler.HandleAsync(
            new ObtenerPlatosDisponiblesQuery(),
            cancellationToken
        );
        return Ok(platos);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(
        [FromServices] ObtenerPlatosHandler handler,
        CancellationToken cancellationToken
    )
    {
        var platos = await handler.HandleAsync(new ObtenerPlatosQuery(), cancellationToken);
        return Ok(platos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(
        Guid id,
        [FromServices] ObtenerPlatoPorIdHandler handler,
        CancellationToken cancellationToken
    )
    {
        var plato = await handler.HandleAsync(new ObtenerPlatoPorIdQuery(id), cancellationToken);
        return plato is null ? ResultExtensions.ToNotFoundResult("Plato", id) : Ok(plato);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearPlatoCommand command,
        [FromServices] CrearPlatoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToCreatedResult(plato =>
            Url.Action(nameof(ObtenerPorId), new { id = plato.Id })!
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarPlatoRequest request,
        [FromServices] ActualizarPlatoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var command = new ActualizarPlatoCommand(
            id,
            request.Nombre,
            request.Descripcion,
            request.Precio,
            request.Categoria,
            request.Disponible
        );

        var result = await handler.HandleAsync(command, cancellationToken);
        return result.ToOkResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(
        Guid id,
        [FromServices] EliminarPlatoHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new EliminarPlatoCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}

/// Cuerpo de solicitud para actualizar un plato.
public sealed record ActualizarPlatoRequest(
    string Nombre,
    string Descripcion,
    decimal Precio,
    Domain.Enums.CategoriaPlato Categoria,
    bool Disponible
);
