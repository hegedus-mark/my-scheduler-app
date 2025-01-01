using SharedKernel.Domain.Interfaces;
using SharedKernel.Domain.ValueObjects;

namespace Scheduling.Domain.Events;

public class TaskScheduledEvent : IDomainEvent
{
    public TaskScheduledEvent(Guid id, CalendarTimeWindow scheduledTimeWindow)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
