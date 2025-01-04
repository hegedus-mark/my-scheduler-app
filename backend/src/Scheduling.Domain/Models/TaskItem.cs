using Scheduling.Domain.Enums;
using Scheduling.Domain.Events;
using Scheduling.Domain.Services;
using SharedKernel.Common.Guard;
using SharedKernel.Common.Results;
using SharedKernel.Domain.Base;
using SharedKernel.Domain.ValueObjects;

namespace Scheduling.Domain.Models;

public class TaskItem : AggregateRoot
{
    private TaskItem(
        string name,
        DateTime dueDate,
        TimeSpan duration,
        PriorityLevel priority,
        Guid? id = null
    )
        : base(id)
    {
        Guard.AgainstNullOrEmpty(name, nameof(name));

        if (dueDate <= DateTime.Now)
            throw new ArgumentException("Due date must be in the future");

        Name = name;
        DueDate = dueDate;
        Duration = duration;
        Priority = priority;
        State = new DraftState(this);
    }

    public CalendarTimeWindow? ScheduledTime { get; private set; }

    public string Name { get; }
    public DateTime DueDate { get; }
    public TimeSpan Duration { get; }
    public PriorityLevel Priority { get; }

    private TaskState State { get; set; }
    public string? FailureReason { get; private set; }

    public TaskItemStatus Status => GetStatus();

    public bool IsScheduled => State is ScheduledState;
    public bool IsDraft => State is DraftState;
    public bool HasFailed => State is FailedToScheduleState;

    // Factory method for creating new tasks
    public static TaskItem Create(
        string name,
        DateTime dueDate,
        TimeSpan duration,
        PriorityLevel priority
    )
    {
        return new TaskItem(name, dueDate, duration, priority);
    }

    public static TaskItem Load(
        string name,
        DateTime dueDate,
        TimeSpan duration,
        PriorityLevel priority,
        Guid id
    )
    {
        return new TaskItem(name, dueDate, duration, priority, id);
    }

    private TaskItemStatus GetStatus()
    {
        if (IsDraft)
            return TaskItemStatus.Draft;
        if (HasFailed)
            return TaskItemStatus.Unscheduled;

        if (IsScheduled)
            return TaskItemStatus.Scheduled;

        throw new InvalidOperationException("Unknown TaskItemState");
    }

    public int CalculateScore(IScoringStrategy scoringStrategy)
    {
        return scoringStrategy.CalculateScore(this);
    }

    public Result Schedule(CalendarTimeWindow scheduleTimeWindow)
    {
        return State.Schedule(scheduleTimeWindow);
    }

    public Result MarkAsFailedToSchedule(string reason)
    {
        return State.MarkAsFailed(reason);
    }

    public Result RetryScheduling()
    {
        return State.RetryScheduling();
    }

    // Internal state transition methods called by the state classes
    private void TransitionToScheduled(CalendarTimeWindow scheduledTimeWindow)
    {
        ScheduledTime = scheduledTimeWindow;
        State = new ScheduledState(this);
        AddDomainEvent(new TaskScheduledEvent(Id, scheduledTimeWindow));
    }

    private void TransitionToFailed(string reason)
    {
        FailureReason = reason;
        State = new FailedToScheduleState(this);
        AddDomainEvent(new TaskFailedToScheduleEvent(Id, reason));
    }

    private void TransitionToDraft()
    {
        FailureReason = null;
        State = new DraftState(this);
        AddDomainEvent(new TaskSchedulingRetryRequestedEvent(Id));
    }

    private abstract class TaskState
    {
        protected readonly TaskItem Task;

        protected TaskState(TaskItem task)
        {
            Task = task;
        }

        public abstract Result Schedule(CalendarTimeWindow scheduleTimeWindow);
        public abstract Result MarkAsFailed(string reason);
        public abstract Result RetryScheduling();
    }

    private class DraftState : TaskState
    {
        public DraftState(TaskItem task)
            : base(task) { }

        public override Result Schedule(CalendarTimeWindow scheduleTimeWindow)
        {
            Task.TransitionToScheduled(scheduleTimeWindow);
            return Result.Success();
        }

        public override Result MarkAsFailed(string reason)
        {
            Task.TransitionToFailed(reason);
            return Result.Success();
        }

        public override Result RetryScheduling()
        {
            return Result.Failure("Task is already in draft state");
        }
    }

    private class ScheduledState : TaskState
    {
        public ScheduledState(TaskItem task)
            : base(task) { }

        public override Result Schedule(CalendarTimeWindow scheduledTimeWindow)
        {
            return Result.Failure("Task is already scheduled");
        }

        public override Result MarkAsFailed(string reason)
        {
            Task.TransitionToFailed(reason);
            return Result.Success();
        }

        public override Result RetryScheduling()
        {
            return Result.Failure("Cannot retry - task is already scheduled");
        }
    }

    private class FailedToScheduleState : TaskState
    {
        public FailedToScheduleState(TaskItem task)
            : base(task) { }

        public override Result Schedule(CalendarTimeWindow scheduledTimeWindow)
        {
            return Result.Failure("Failed task must be reset to draft state before scheduling");
        }

        public override Result MarkAsFailed(string reason)
        {
            return Result.Failure("Task is already marked as failed");
        }

        public override Result RetryScheduling()
        {
            Task.TransitionToDraft();
            return Result.Success();
        }
    }
}
