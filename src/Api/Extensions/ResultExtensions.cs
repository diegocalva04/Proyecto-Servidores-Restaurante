using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

/// Convierte resultados del dominio/aplicación en respuestas HTTP.
public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        return ToErrorResult(result.Error);
    }

    public static IActionResult ToCreatedResult<T>(
        this Result<T> result,
        Func<T, string> locationFactory,
        Func<T, object>? responseFactory = null
    )
    {
        if (result.IsFailure)
        {
            return ToErrorResult(result.Error);
        }

        var location = locationFactory(result.Value);
        var body = responseFactory?.Invoke(result.Value) ?? result.Value;
        return new CreatedResult(location, body);
    }

    public static IActionResult ToOkResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return ToErrorResult(result.Error);
    }

    private static IActionResult ToErrorResult(Error error)
    {
        if (error.Code.EndsWith(".NoEncontrado", StringComparison.Ordinal))
        {
            return new NotFoundObjectResult(
                new ProblemDetails
                {
                    Title = "Recurso no encontrado",
                    Detail = error.Message,
                    Status = StatusCodes.Status404NotFound,
                    Extensions = { ["code"] = error.Code },
                }
            );
        }

        return new BadRequestObjectResult(
            new ProblemDetails
            {
                Title = "Error de validación",
                Detail = error.Message,
                Status = StatusCodes.Status400BadRequest,
                Extensions = { ["code"] = error.Code },
            }
        );
    }
}
