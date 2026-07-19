using Application.Clientes.Dtos;
using Application.Common;
using Application.Common.Errors;
using Application.Common.Mapping;
using Domain.Common;
using Domain.Repositories;

namespace Application.Clientes.Commands.ActualizarCliente;

public sealed class ActualizarClienteHandler(
    IClienteRepository clienteRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ActualizarClienteCommand, Result<ClienteDto>>
{
    public async Task<Result<ClienteDto>> HandleAsync(
        ActualizarClienteCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var cliente = await clienteRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (cliente is null)
        {
            return Result.Failure<ClienteDto>(
                ApplicationErrors.NoEncontrado("Cliente", command.Id)
            );
        }

        var actualizarResult = cliente.Actualizar(
            command.Nombre,
            command.Apellido,
            command.Correo,
            command.Telefono
        );

        if (actualizarResult.IsFailure)
        {
            return Result.Failure<ClienteDto>(actualizarResult.Error);
        }

        await clienteRepository.ActualizarAsync(cliente, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(cliente.ToDto());
    }
}
