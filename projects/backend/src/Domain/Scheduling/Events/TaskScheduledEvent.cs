using CalendarTimeWindow = Domain.Shared.ValueObjects.CalendarTimeWindow;
using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;

namespace Domain.Scheduling.Events;

public record TaskScheduledEvent(Guid TaskId, CalendarTimeWindow ScheduledTimeWindow) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
