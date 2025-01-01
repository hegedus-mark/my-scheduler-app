using SharedKernel.Domain.Interfaces;

namespace Scheduling.Domain.Events;

public class TaskFailedToScheduleEvent : IDomainEvent
{
    public TaskFailedToScheduleEvent(Guid id, string reason)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
