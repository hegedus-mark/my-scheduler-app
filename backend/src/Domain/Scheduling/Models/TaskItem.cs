using Domain.Scheduling.Events;
using Domain.Scheduling.Models.Enums;
using Domain.Scheduling.Models.States;
using Domain.Scheduling.Services;
using SharedKernel.Common.Guard;
using SharedKernel.Domain.Base;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Models;

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

    public void Schedule(CalendarTimeWindow scheduleTimeWindow)
    {
        State.Schedule(scheduleTimeWindow);
    }

    public void MarkAsFailedToSchedule(string reason)
    {
        State.MarkAsFailed(reason);
    }

    public void RetryScheduling()
    {
        State.RetryScheduling();
    }

    // Internal state transition methods called by the state classes
    internal void TransitionToScheduled(CalendarTimeWindow scheduledTimeWindow)
    {
        ScheduledTime = scheduledTimeWindow;
        State = new ScheduledState(this);
        AddDomainEvent(new TaskScheduledEvent(Id, scheduledTimeWindow));
    }

    internal void TransitionToFailed(string reason)
    {
        FailureReason = reason;
        State = new FailedToScheduleState(this);
        AddDomainEvent(new TaskFailedToScheduleEvent(Id, reason));
    }

    internal void TransitionToDraft()
    {
        FailureReason = null;
        State = new DraftState(this);
        AddDomainEvent(new TaskSchedulingRetryRequestedEvent(Id));
    }
}
