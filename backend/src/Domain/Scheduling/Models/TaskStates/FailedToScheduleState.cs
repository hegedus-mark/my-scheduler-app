using Domain.Scheduling.Exceptions;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Models.States;

public class FailedToScheduleState : TaskState
{
    public FailedToScheduleState(TaskItem task)
        : base(task) { }

    public override void Schedule(CalendarTimeWindow scheduledTimeWindow)
    {
        throw new TaskStateTransitionException(
            nameof(FailedToScheduleState),
            "Attempted to Schedule FailedToSchedule Task, Task must be reset to Draft first"
        );
    }

    public override void MarkAsFailed(string reason)
    {
        throw new TaskStateTransitionException(
            nameof(FailedToScheduleState),
            "Attempted to mark as failed already failedToSchedule task"
        );
    }

    public override void RetryScheduling()
    {
        Task.TransitionToDraft();
    }
}
