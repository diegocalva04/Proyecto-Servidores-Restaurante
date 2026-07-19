namespace Application.Clientes.Commands.ActualizarCliente;

public sealed record ActualizarClienteCommand(
    Guid Id,
    string Nombre,
    string Apellido,
    string Correo,
    string Telefono
);
