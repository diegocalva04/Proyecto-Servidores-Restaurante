using Application.Pedidos.Dtos;
using Domain.Entities;

namespace Application.Common.Mapping;

internal static class PedidoMappings
{
    internal static PedidoDto ToDto(this Pedido pedido) =>
        new(
            pedido.Id,
            pedido.ClienteId,
            pedido.Fecha,
            pedido.Estado.ToString(),
            pedido.Total.Valor,
            pedido.Lineas.Select(l => l.ToDto()).ToList()
        );

    internal static PedidoLineaDto ToDto(this PedidoLinea linea) =>
        new(
            linea.Id,
            linea.PlatoId,
            linea.NombrePlato,
            linea.PrecioUnitario.Valor,
            linea.Cantidad,
            linea.Subtotal.Valor
        );
}
