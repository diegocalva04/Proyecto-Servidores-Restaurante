using Application.Clientes.Dtos;
using Domain.Entities;

namespace Application.Common.Mapping;

internal static class ClienteMappings
{
    internal static ClienteDto ToDto(this Cliente cliente) =>
        new(
            cliente.Id,
            cliente.NombreCompleto.Nombre,
            cliente.NombreCompleto.Apellido,
            cliente.Correo.Valor,
            cliente.Telefono.Valor
        );
}
