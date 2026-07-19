namespace Domain.Enums;

/// Estados posibles de un pedido a lo largo de su ciclo de vida.
public enum EstadoPedido
{
    Pendiente = 1,
    EnPreparacion = 2,
    Entregado = 3,
    Cancelado = 4,
}
