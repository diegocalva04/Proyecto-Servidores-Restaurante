namespace Application.Common.Errors;

/// Errores de orquestación propios de la capa de aplicación.
public static class ApplicationErrors
{
    public static Domain.Common.Error NoEncontrado(string entidad, Guid id) =>
        new($"{entidad}.NoEncontrado", $"{entidad} con id '{id}' no fue encontrado.");
}
