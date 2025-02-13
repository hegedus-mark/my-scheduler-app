using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;

namespace Domain.Scheduling.Events;

public record TaskFailedToScheduleEvent(Guid TaskId, string Reason) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
