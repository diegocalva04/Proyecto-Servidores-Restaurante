using Domain.Common;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

/// Representa un cliente registrado del restaurante.
public sealed class Cliente : AggregateRoot
{
    private Cliente(
        Guid id,
        NombreCompleto nombreCompleto,
        CorreoElectronico correo,
        Telefono telefono
    )
        : base(id)
    {
        NombreCompleto = nombreCompleto;
        Correo = correo;
        Telefono = telefono;
    }

    public NombreCompleto NombreCompleto { get; private set; }

    public CorreoElectronico Correo { get; private set; }

    public Telefono Telefono { get; private set; }

    /// Crea un cliente validando nombre, correo y teléfono mediante value objects.
    public static Result<Cliente> Crear(
        string nombre,
        string apellido,
        string correo,
        string telefono
    )
    {
        var nombreResult = NombreCompleto.Crear(nombre, apellido);
        if (nombreResult.IsFailure)
        {
            return Result.Failure<Cliente>(nombreResult.Error);
        }

        var correoResult = CorreoElectronico.Crear(correo);
        if (correoResult.IsFailure)
        {
            return Result.Failure<Cliente>(correoResult.Error);
        }

        var telefonoResult = Telefono.Crear(telefono);
        if (telefonoResult.IsFailure)
        {
            return Result.Failure<Cliente>(telefonoResult.Error);
        }

        var cliente = new Cliente(
            Guid.NewGuid(),
            nombreResult.Value,
            correoResult.Value,
            telefonoResult.Value
        );

        cliente.RaiseDomainEvent(
            new ClienteRegistrado(
                cliente.Id,
                cliente.NombreCompleto.Nombre,
                cliente.NombreCompleto.Apellido,
                cliente.Correo.Valor,
                DateTimeOffset.UtcNow
            )
        );

        return Result.Success(cliente);
    }

    public Result Actualizar(string nombre, string apellido, string correo, string telefono)
    {
        var nombreResult = NombreCompleto.Crear(nombre, apellido);
        if (nombreResult.IsFailure)
        {
            return Result.Failure(nombreResult.Error);
        }

        var correoResult = CorreoElectronico.Crear(correo);
        if (correoResult.IsFailure)
        {
            return Result.Failure(correoResult.Error);
        }

        var telefonoResult = Telefono.Crear(telefono);
        if (telefonoResult.IsFailure)
        {
            return Result.Failure(telefonoResult.Error);
        }

        NombreCompleto = nombreResult.Value;
        Correo = correoResult.Value;
        Telefono = telefonoResult.Value;

        return Result.Success();
    }

    private Cliente()
        : base()
    {
        NombreCompleto = NombreCompleto.Crear("Ef", "Core").Value;
        Correo = CorreoElectronico.Crear("ef@local.dev").Value;
        Telefono = Telefono.Crear("1234567").Value;
    }
}
