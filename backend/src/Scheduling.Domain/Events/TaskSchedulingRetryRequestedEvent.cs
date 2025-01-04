using SharedKernel.Domain.Interfaces;

namespace Scheduling.Domain.Events;

public class TaskSchedulingRetryRequestedEvent : IDomainEvent
{
    public TaskSchedulingRetryRequestedEvent(Guid id)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
