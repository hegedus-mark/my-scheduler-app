using SharedKernel.Domain.Interfaces;

namespace Domain.Scheduling.Events;

public class TaskSchedulingRetryRequestedEvent : IDomainEvent
{
    public TaskSchedulingRetryRequestedEvent(Guid id)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
