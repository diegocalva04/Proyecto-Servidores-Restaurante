namespace Domain.Common;

/// Marca un evento ocurrido dentro del dominio.
public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
