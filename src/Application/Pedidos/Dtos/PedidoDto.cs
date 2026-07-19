namespace Application.Pedidos.Dtos;

/// DTO de lectura para un pedido.
public sealed record PedidoDto(
    Guid Id,
    Guid ClienteId,
    DateTimeOffset Fecha,
    string Estado,
    decimal Total,
    IReadOnlyList<PedidoLineaDto> Lineas
);
