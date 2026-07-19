using Domain.Enums;

namespace Application.Platos.Commands.ActualizarPlato;

public sealed record ActualizarPlatoCommand(
    Guid Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    CategoriaPlato Categoria,
    bool Disponible
);
