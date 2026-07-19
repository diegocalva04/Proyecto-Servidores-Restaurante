using Domain.Common;

namespace Domain.Events;

/// Evento emitido cuando se registra un nuevo cliente en el sistema.
public sealed record ClienteRegistrado(
    Guid ClienteId,
    string Nombre,
    string Apellido,
    string Correo,
    DateTimeOffset OccurredOn
) : IDomainEvent;
