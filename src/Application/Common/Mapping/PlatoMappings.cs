using Application.Platos.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Mapping;

/// Mapea entidades de dominio a DTOs de la capa de aplicación.
internal static class PlatoMappings
{
    internal static PlatoDto ToDto(this Plato plato) =>
        new(
            plato.Id,
            plato.Nombre,
            plato.Descripcion,
            plato.Precio.Valor,
            plato.Categoria.ToString(),
            plato.Disponible
        );
}
