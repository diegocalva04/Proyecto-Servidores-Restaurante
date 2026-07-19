namespace Domain.Common;

/// Raíz de agregado: punto de entrada para garantizar invariantes del agregado.
public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id)
        : base(id) { }

    protected AggregateRoot() { }
}
