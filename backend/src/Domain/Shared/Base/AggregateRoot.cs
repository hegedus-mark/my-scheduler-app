using Domain.Shared.Interfaces;

namespace Domain.Shared.Base;

public class AggregateRoot : EntityBase, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(Guid? id = null)
        : base(id) { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
