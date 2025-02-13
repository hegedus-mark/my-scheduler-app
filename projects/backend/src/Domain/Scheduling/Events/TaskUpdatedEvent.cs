using Domain.Shared.Interfaces;

namespace Domain.Scheduling.Events;

public record TaskUpdatedEvent(Guid Id, string Property, object NewValue) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
