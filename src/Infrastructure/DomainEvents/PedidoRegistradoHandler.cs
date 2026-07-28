using Domain.Common;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DomainEvents;

public sealed class PedidoRegistradoHandler(ILogger<PedidoRegistradoHandler> logger)
    : IDomainEventHandler<PedidoRegistrado>
{
    public Task HandleAsync(PedidoRegistrado domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evento PedidoRegistrado procesado para {PedidoId}", domainEvent.PedidoId);
        return Task.CompletedTask;
    }
}
