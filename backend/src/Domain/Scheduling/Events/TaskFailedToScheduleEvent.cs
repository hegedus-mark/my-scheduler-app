using SharedKernel.Domain.Interfaces;

namespace Domain.Scheduling.Events;

public class TaskFailedToScheduleEvent : IDomainEvent
{
    public TaskFailedToScheduleEvent(Guid id, string reason)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
