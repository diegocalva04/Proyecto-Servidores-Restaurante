namespace Application.Clientes.Commands.CrearCliente;

/// <summary>
/// Comando principal de escritura: registrar un nuevo cliente.
/// </summary>
public sealed record CrearClienteCommand(
    string Nombre,
    string Apellido,
    string Correo,
    string Telefono
);
