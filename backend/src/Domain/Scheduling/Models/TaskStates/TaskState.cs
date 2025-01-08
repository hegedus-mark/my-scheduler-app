using Domain.Shared.ValueObjects;

namespace Domain.Scheduling.Models.TaskStates;

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
