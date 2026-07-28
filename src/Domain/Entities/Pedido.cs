using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

/// Agregado raíz que representa un pedido realizado por un cliente.
/// Contiene las reglas de negocio del proceso de pedido.
public sealed class Pedido : AggregateRoot
{
    private readonly List<PedidoLinea> _lineas = [];

    private Pedido(
        Guid id,
        Guid clienteId,
        DateTimeOffset fecha,
        EstadoPedido estado,
        Precio total,
        IEnumerable<PedidoLinea> lineas
    )
        : base(id)
    {
        ClienteId = clienteId;
        Fecha = fecha;
        Estado = estado;
        Total = total;
        _lineas.AddRange(lineas);
    }

    public Guid ClienteId { get; private set; }

    public DateTimeOffset Fecha { get; private set; }

    public EstadoPedido Estado { get; private set; }

    public Precio Total { get; private set; }

    public IReadOnlyCollection<PedidoLinea> Lineas => _lineas.AsReadOnly();

    /// <summary>
    /// Crea un pedido validando cliente, platos, disponibilidad y calculando el total.
    /// </summary>
    public static Result<Pedido> Crear(
        Guid clienteId,
        IReadOnlyList<SolicitudLineaPedido> solicitudes,
        IReadOnlyList<Plato> platos,
        DateTimeOffset fecha
    )
    {
        if (clienteId == Guid.Empty)
        {
            return Result.Failure<Pedido>(DomainErrors.Pedido.ClienteRequerido);
        }

        if (solicitudes.Count == 0)
        {
            return Result.Failure<Pedido>(DomainErrors.Pedido.SinPlatos);
        }

        var lineasResult = ConstruirLineas(solicitudes, platos);
        if (lineasResult.IsFailure)
        {
            return Result.Failure<Pedido>(lineasResult.Error);
        }

        var total = CalcularTotal(lineasResult.Value);
        var pedido = new Pedido(
            Guid.NewGuid(),
            clienteId,
            fecha,
            EstadoPedido.Pendiente,
            total,
            lineasResult.Value
        );

        pedido.RaiseDomainEvent(
            new PedidoRegistrado(
                pedido.Id,
                pedido.ClienteId,
                pedido.Total.Valor,
                pedido.Fecha,
                DateTimeOffset.UtcNow
            )
        );

        return Result.Success(pedido);
    }

    /// <summary>
    /// Actualiza el estado del pedido respetando la regla de pedidos entregados.
    /// </summary>
    public Result ActualizarEstado(EstadoPedido nuevoEstado)
    {
        if (EsEstadoFinal)
        {
            return Result.Failure(DomainErrors.Pedido.EstadoFinal);
        }

        if (!Enum.IsDefined(nuevoEstado))
        {
            return Result.Failure(DomainErrors.Pedido.EstadoInvalido);
        }

        var transicionValida = Estado switch
        {
            EstadoPedido.Pendiente => nuevoEstado is EstadoPedido.EnPreparacion or EstadoPedido.Cancelado,
            EstadoPedido.EnPreparacion => nuevoEstado is EstadoPedido.Entregado or EstadoPedido.Cancelado,
            _ => false,
        };

        if (!transicionValida)
        {
            return Result.Failure(DomainErrors.Pedido.EstadoInvalido);
        }

        Estado = nuevoEstado;
        return Result.Success();
    }

    /// <summary>
    /// Actualiza las líneas del pedido recalculando el total.
    /// </summary>
    public Result ActualizarLineas(
        IReadOnlyList<SolicitudLineaPedido> solicitudes,
        IReadOnlyList<Plato> platos
    )
    {
        if (EsEstadoFinal)
        {
            return Result.Failure(DomainErrors.Pedido.EstadoFinal);
        }

        if (solicitudes.Count == 0)
        {
            return Result.Failure(DomainErrors.Pedido.SinPlatos);
        }

        var lineasResult = ConstruirLineas(solicitudes, platos);
        if (lineasResult.IsFailure)
        {
            return Result.Failure(lineasResult.Error);
        }

        _lineas.Clear();
        _lineas.AddRange(lineasResult.Value);
        Total = CalcularTotal(_lineas);

        return Result.Success();
    }

    public bool EstaEntregado => Estado == EstadoPedido.Entregado;

    public bool EsEstadoFinal => Estado is EstadoPedido.Entregado or EstadoPedido.Cancelado;

    private static Result<List<PedidoLinea>> ConstruirLineas(
        IReadOnlyList<SolicitudLineaPedido> solicitudes,
        IReadOnlyList<Plato> platos
    )
    {
        var platosPorId = platos.ToDictionary(p => p.Id);
        var lineas = new List<PedidoLinea>();

        foreach (var solicitud in solicitudes)
        {
            if (solicitud.Cantidad <= 0)
            {
                return Result.Failure<List<PedidoLinea>>(DomainErrors.Pedido.CantidadInvalida);
            }

            if (!platosPorId.TryGetValue(solicitud.PlatoId, out var plato))
            {
                return Result.Failure<List<PedidoLinea>>(DomainErrors.Pedido.PlatoNoEncontrado);
            }

            if (!plato.Disponible)
            {
                return Result.Failure<List<PedidoLinea>>(DomainErrors.Pedido.PlatoNoDisponible);
            }

            lineas.Add(
                new PedidoLinea(
                    Guid.NewGuid(),
                    plato.Id,
                    plato.Nombre,
                    plato.Precio,
                    solicitud.Cantidad
                )
            );
        }

        return Result.Success(lineas);
    }

    private static Precio CalcularTotal(IEnumerable<PedidoLinea> lineas)
    {
        var total = Precio.Zero;

        foreach (var linea in lineas)
        {
            total = total.Sumar(linea.Subtotal);
        }

        return total;
    }

    private Pedido()
        : base()
    {
        Total = Precio.Zero;
    }
}
