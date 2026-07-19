using Application.Common;
using Application.Common.Errors;
using Domain.Common;
using Domain.Repositories;

namespace Application.Clientes.Commands.EliminarCliente;

public sealed class EliminarClienteHandler(
    IClienteRepository clienteRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<EliminarClienteCommand, Result>
{
    public async Task<Result> HandleAsync(
        EliminarClienteCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var cliente = await clienteRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (cliente is null)
        {
            return Result.Failure(ApplicationErrors.NoEncontrado("Cliente", command.Id));
        }

        await clienteRepository.EliminarAsync(cliente, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success();
    }
}
