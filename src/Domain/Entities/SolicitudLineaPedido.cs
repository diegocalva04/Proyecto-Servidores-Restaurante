namespace Domain.Entities;

/// Representa la solicitud de una línea al crear o actualizar un pedido.
public sealed record SolicitudLineaPedido(Guid PlatoId, int Cantidad);
