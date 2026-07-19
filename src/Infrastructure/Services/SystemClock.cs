using Domain.Services;

namespace Infrastructure.Services;

/// Implementación del reloj del sistema para la capa de infraestructura.
public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
