using Application.Common;
using Application.Common.Errors;
using Application.Common.Mapping;
using Application.Platos.Dtos;
using Domain.Common;
using Domain.Repositories;

namespace Application.Platos.Commands.ActualizarPlato;

public sealed class ActualizarPlatoHandler(IPlatoRepository platoRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ActualizarPlatoCommand, Result<PlatoDto>>
{
    public async Task<Result<PlatoDto>> HandleAsync(
        ActualizarPlatoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var plato = await platoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);
        if (plato is null)
        {
            return Result.Failure<PlatoDto>(ApplicationErrors.NoEncontrado("Plato", command.Id));
        }

        var actualizarResult = plato.Actualizar(
            command.Nombre,
            command.Descripcion,
            command.Precio,
            command.Categoria,
            command.Disponible
        );

        if (actualizarResult.IsFailure)
        {
            return Result.Failure<PlatoDto>(actualizarResult.Error);
        }

        await platoRepository.ActualizarAsync(plato, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(plato.ToDto());
    }
}
