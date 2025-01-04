using Domain.Scheduling.Exceptions;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Models.States;

public class DraftState : TaskState
{
    public DraftState(TaskItem task)
        : base(task) { }

    public override void Schedule(CalendarTimeWindow scheduleTimeWindow)
    {
        Task.TransitionToScheduled(scheduleTimeWindow);
    }

    public override void MarkAsFailed(string reason)
    {
        Task.TransitionToFailed(reason);
    }

    public override void RetryScheduling()
    {
        throw new TaskStateTransitionException(
            nameof(DraftState),
            "Attempted to retry scheduling on Draft Task"
        );
    }
}
