namespace Application.Platos.Dtos;

/// DTO de lectura para un plato del menú.
public sealed record PlatoDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    string Categoria,
    bool Disponible
);
