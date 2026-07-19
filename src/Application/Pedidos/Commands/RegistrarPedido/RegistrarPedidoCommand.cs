using Application.Pedidos.Dtos;

namespace Application.Pedidos.Commands.RegistrarPedido;

/// Comando principal de toma de decisiones: registrar un pedido con reglas de negocio.
public sealed record RegistrarPedidoCommand(
    Guid ClienteId,
    IReadOnlyList<LineaPedidoRequest> Lineas
);
