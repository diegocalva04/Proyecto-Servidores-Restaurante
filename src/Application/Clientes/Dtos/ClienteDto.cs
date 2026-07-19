namespace Application.Clientes.Dtos;

/// <summary>
/// DTO de lectura para un cliente registrado.
/// </summary>
public sealed record ClienteDto(
    Guid Id,
    string Nombre,
    string Apellido,
    string Correo,
    string Telefono
);
