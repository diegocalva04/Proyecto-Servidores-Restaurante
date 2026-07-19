namespace Application.Pedidos.Dtos;

/// DTO de lectura para una línea de pedido.
public sealed record PedidoLineaDto(
    Guid Id,
    Guid PlatoId,
    string NombrePlato,
    decimal PrecioUnitario,
    int Cantidad,
    decimal Subtotal
);
