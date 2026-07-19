using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DomainValueObjects = Domain.ValueObjects;

namespace Infrastructure.Persistence.Conversions;

/// Conversores estáticos para mapear value objects con EF Core.
internal static class ValueConverters
{
    internal static readonly ValueConverter<
        DomainValueObjects.NombreCompleto,
        string
    > NombreCompleto = new(
        nombre => nombre.Nombre + "|" + nombre.Apellido,
        valor =>
            DomainValueObjects
                .NombreCompleto.Crear(
                    valor.Substring(0, valor.IndexOf('|')),
                    valor.Substring(valor.IndexOf('|') + 1)
                )
                .Value
    );

    internal static readonly ValueConverter<DomainValueObjects.CorreoElectronico, string> Correo =
        new(
            correo => correo.Valor,
            valor => DomainValueObjects.CorreoElectronico.Crear(valor).Value
        );

    internal static readonly ValueConverter<DomainValueObjects.Telefono, string> Telefono = new(
        telefono => telefono.Valor,
        valor => DomainValueObjects.Telefono.Crear(valor).Value
    );

    internal static readonly ValueConverter<DomainValueObjects.Precio, decimal> Precio = new(
        precio => precio.Valor,
        valor => DomainValueObjects.Precio.Crear(valor).Value
    );
}
