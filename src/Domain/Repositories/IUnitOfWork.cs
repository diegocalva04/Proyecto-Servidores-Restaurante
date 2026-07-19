namespace Domain.Repositories;

/// Coordina la persistencia transaccional de los cambios del dominio.
public interface IUnitOfWork
{
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
