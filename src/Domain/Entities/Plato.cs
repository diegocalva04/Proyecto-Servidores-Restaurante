using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;

namespace Domain.Entities;

/// Representa un plato del menú del restaurante.
public sealed class Plato : AggregateRoot
{
    private Plato(
        Guid id,
        string nombre,
        string descripcion,
        Precio precio,
        CategoriaPlato categoria,
        bool disponible
    )
        : base(id)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Precio = precio;
        Categoria = categoria;
        Disponible = disponible;
    }

    public string Nombre { get; private set; }

    public string Descripcion { get; private set; }

    public Precio Precio { get; private set; }

    public CategoriaPlato Categoria { get; private set; }

    public bool Disponible { get; private set; }

    public static Result<Plato> Crear(
        string nombre,
        string descripcion,
        decimal precio,
        CategoriaPlato categoria,
        bool disponible = true
    )
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return Result.Failure<Plato>(DomainErrors.Plato.NombreInvalido);
        }

        var precioResult = Precio.Crear(precio);
        if (precioResult.IsFailure)
        {
            return Result.Failure<Plato>(precioResult.Error);
        }

        var plato = new Plato(
            Guid.NewGuid(),
            nombre.Trim(),
            descripcion?.Trim() ?? string.Empty,
            precioResult.Value,
            categoria,
            disponible
        );

        return Result.Success(plato);
    }

    public Result Actualizar(
        string nombre,
        string descripcion,
        decimal precio,
        CategoriaPlato categoria,
        bool disponible
    )
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return Result.Failure(DomainErrors.Plato.NombreInvalido);
        }

        var precioResult = Precio.Crear(precio);
        if (precioResult.IsFailure)
        {
            return Result.Failure(precioResult.Error);
        }

        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim() ?? string.Empty;
        Precio = precioResult.Value;
        Categoria = categoria;
        Disponible = disponible;

        return Result.Success();
    }

    public void MarcarComoDisponible() => Disponible = true;

    public void MarcarComoNoDisponible() => Disponible = false;

    private Plato()
        : base()
    {
        Nombre = string.Empty;
        Descripcion = string.Empty;
        Precio = Precio.Zero;
    }
}
