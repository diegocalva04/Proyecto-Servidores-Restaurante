using Domain.Enums;

namespace Application.Platos.Commands.CrearPlato;

public sealed record CrearPlatoCommand(
    string Nombre,
    string Descripcion,
    decimal Precio,
    CategoriaPlato Categoria,
    bool Disponible = true
);
