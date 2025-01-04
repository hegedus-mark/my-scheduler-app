using Domain.Scheduling.Exceptions;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Models.States;

public class ScheduledState : TaskState
{
    public ScheduledState(TaskItem task)
        : base(task) { }

    public override void Schedule(CalendarTimeWindow scheduledTimeWindow)
    {
        throw new TaskStateTransitionException(
            nameof(ScheduledState),
            "Attempted to schedule already scheduled task"
        );
    }

    public override void MarkAsFailed(string reason)
    {
        Task.TransitionToFailed(reason);
    }

    public override void RetryScheduling()
    {
        throw new TaskStateTransitionException(
            nameof(ScheduledState),
            "Attempted to retry scheduling already scheduled Task"
        );
    }
}
