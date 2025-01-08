using Domain.Shared.Interfaces;

namespace Domain.Scheduling.Events;

public class TaskUpdatedEvent : IDomainEvent
{
    public TaskUpdatedEvent(Guid id, string property, object newProperty) { }

    public DateTime OccurredOn => DateTime.Now;
}
