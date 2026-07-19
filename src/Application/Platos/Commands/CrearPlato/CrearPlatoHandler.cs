using Application.Common;
using Application.Common.Mapping;
using Application.Platos.Dtos;
using Domain.Common;
using Domain.Repositories;

namespace Application.Platos.Commands.CrearPlato;

public sealed class CrearPlatoHandler(IPlatoRepository platoRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CrearPlatoCommand, Result<PlatoDto>>
{
    public async Task<Result<PlatoDto>> HandleAsync(
        CrearPlatoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var platoResult = Domain.Entities.Plato.Crear(
            command.Nombre,
            command.Descripcion,
            command.Precio,
            command.Categoria,
            command.Disponible
        );

        if (platoResult.IsFailure)
        {
            return Result.Failure<PlatoDto>(platoResult.Error);
        }

        await platoRepository.AgregarAsync(platoResult.Value, cancellationToken);
        await unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Success(platoResult.Value.ToDto());
    }
}
