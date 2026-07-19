using Application.Clientes.Dtos;
using Application.Common;
using Application.Common.Mapping;
using Domain.Common;
using Domain.Repositories;

namespace Application.Clientes.Commands.CrearCliente;

/// <summary>
/// Handler del caso de uso de escritura: registrar cliente mediante fábrica de dominio.
/// </summary>
public sealed class CrearClienteHandler(
    IClienteRepository clienteRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CrearClienteCommand, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> HandleAsync(
        CrearClienteCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var clienteResult = Domain.Entities.Cliente.Crear(
            command.Nombre,
            command.Apellido,
            command.Correo,
            command.Telefono
        );

        if (clienteResult.IsFailure)
        {
            return Result.Failure<ClienteDto>(clienteResult.Error);
        }

        await clienteRepository.AgregarAsync(clienteResult.Value, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(clienteResult.Value.ToDto());
    }
}
