using Domain.Common;

namespace Domain.Events;

/// Evento emitido cuando se confirma un nuevo pedido.
public sealed record PedidoRegistrado(
    Guid PedidoId,
    Guid ClienteId,
    decimal Total,
    DateTimeOffset Fecha,
    DateTimeOffset OccurredOn
) : IDomainEvent;
