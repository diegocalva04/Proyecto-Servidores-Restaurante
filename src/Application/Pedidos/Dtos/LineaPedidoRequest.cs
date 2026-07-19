namespace Application.Pedidos.Dtos;

/// Solicitud de línea de pedido desde la capa de aplicación.
public sealed record LineaPedidoRequest(Guid PlatoId, int Cantidad);
