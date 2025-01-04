using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Models.States;

public abstract class TaskState
{
    protected readonly TaskItem Task;

    protected TaskState(TaskItem task)
    {
        Task = task;
    }

    public abstract void Schedule(CalendarTimeWindow scheduleTimeWindow);
    public abstract void MarkAsFailed(string reason);
    public abstract void RetryScheduling();
}
