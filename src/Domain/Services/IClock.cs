namespace Domain.Services;

/// Abstracción del reloj del sistema para desacoplar el dominio del tiempo real.
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
