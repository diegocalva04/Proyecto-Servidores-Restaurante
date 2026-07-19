namespace Application.Common;

/// Contrato base para handlers de comandos (operaciones de escritura).
public interface ICommandHandler<in TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
