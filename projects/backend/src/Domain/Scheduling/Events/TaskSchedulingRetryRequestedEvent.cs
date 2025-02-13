using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;

namespace Domain.Scheduling.Events;

public record TaskSchedulingRetryRequestedEvent(Guid TaskId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
