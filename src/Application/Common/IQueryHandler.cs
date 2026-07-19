namespace Application.Common;

/// Contrato base para handlers de consultas (operaciones de lectura).
public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
