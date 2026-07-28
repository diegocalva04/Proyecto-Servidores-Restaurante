using Domain.Common;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DomainEvents;

public sealed class ClienteRegistradoHandler(ILogger<ClienteRegistradoHandler> logger)
    : IDomainEventHandler<ClienteRegistrado>
{
    public Task HandleAsync(ClienteRegistrado domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Evento ClienteRegistrado procesado para {ClienteId}", domainEvent.ClienteId);
        return Task.CompletedTask;
    }
}
