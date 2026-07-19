using Application.Common;
using Application.Common.Errors;
using Domain.Common;
using Domain.Repositories;

namespace Application.Platos.Commands.EliminarPlato;

public sealed class EliminarPlatoHandler(IPlatoRepository platoRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<EliminarPlatoCommand, Result>
{
    public async Task<Result> HandleAsync(
        EliminarPlatoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var plato = await platoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (plato is null)
        {
            return Result.Failure(ApplicationErrors.NoEncontrado("Plato", command.Id));
        }

        await platoRepository.EliminarAsync(plato, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success();
    }
}
