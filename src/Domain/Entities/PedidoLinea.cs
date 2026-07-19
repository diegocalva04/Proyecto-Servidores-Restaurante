using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

/// Línea de detalle dentro de un pedido. Pertenece al agregado <see cref="Pedido"/>.
public sealed class PedidoLinea : Entity
{
    internal PedidoLinea(
        Guid id,
        Guid platoId,
        string nombrePlato,
        Precio precioUnitario,
        int cantidad
    )
        : base(id)
    {
        PlatoId = platoId;
        NombrePlato = nombrePlato;
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }

    public Guid PlatoId { get; private set; }

    public string NombrePlato { get; private set; }

    public Precio PrecioUnitario { get; private set; }

    public int Cantidad { get; private set; }

    public Precio Subtotal => PrecioUnitario.Multiplicar(Cantidad);

    private PedidoLinea()
        : base()
    {
        NombrePlato = string.Empty;
        PrecioUnitario = Precio.Zero;
    }
}
