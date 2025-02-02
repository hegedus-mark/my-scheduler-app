using CalendarTimeWindow = Domain.Shared.ValueObjects.CalendarTimeWindow;
using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;

namespace Domain.Scheduling.Events;

public class TaskScheduledEvent : IDomainEvent
{
    public TaskScheduledEvent(Guid id, CalendarTimeWindow scheduledTimeWindow)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
