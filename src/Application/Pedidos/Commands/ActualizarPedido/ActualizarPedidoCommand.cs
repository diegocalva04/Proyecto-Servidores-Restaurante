using Application.Pedidos.Dtos;
using Domain.Enums;

namespace Application.Pedidos.Commands.ActualizarPedido;

public sealed record ActualizarPedidoCommand(
    Guid Id,
    EstadoPedido? Estado,
    IReadOnlyList<LineaPedidoRequest>? Lineas
);
