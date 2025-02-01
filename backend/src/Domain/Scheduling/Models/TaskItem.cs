using Domain.Scheduling.Events;
using Domain.Scheduling.Models.Enums;
using Domain.Scheduling.Models.TaskStates;
using Domain.Scheduling.Services;
using Domain.Shared.Base;
using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;
using SharedKernel.Guard;

namespace Domain.Scheduling.Models;

/// <summary>
///     Represents a schedulable task item in the system that follows the state pattern for managing its lifecycle.
///     This class is the aggregate root for the task scheduling domain.
/// </summary>
public class TaskItem : AggregateRoot
{
    /// <summary>
    ///     Initializes a new instance of the TaskItem class.
    /// </summary>
    /// <param name="name">The name of the task. Cannot be null or empty.</param>
    /// <param name="dueDate">The due date by which the task must be completed.</param>
    /// <param name="duration">The expected duration of the task.</param>
    /// <param name="priority">The priority level of the task.</param>
    /// <param name="id">Optional unique identifier. If not provided, a new GUID will be generated.</param>
    /// <exception cref="ArgumentException">Thrown when name is null or empty.</exception>
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

        Name = name;
        DueDate = dueDate;
        Duration = duration;
        Priority = priority;
        State = new DraftState(this);
    }

    /// <summary>
    ///     Gets the scheduled time window for this task, if it has been scheduled.
    /// </summary>
    public CalendarTimeWindow? ScheduledTime { get; private set; }

    /// <summary>
    ///     Gets the name of the task.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the due date by which the task must be completed.
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    ///     Gets the expected duration of the task.
    /// </summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>
    ///     Gets the priority level of the task.
    /// </summary>
    public PriorityLevel Priority { get; private set; }

    /// <summary>
    ///     Gets or sets the current state of the task.
    /// </summary>
    private TaskState State { get; set; }

    /// <summary>
    ///     Gets the reason why the task failed to schedule, if applicable.
    /// </summary>
    public string? FailureReason { get; private set; }

    /// <summary>
    ///     Gets the current status of the task based on its state.
    /// </summary>
    public TaskItemStatus Status => GetStatus();

    /// <summary>
    ///     Gets whether the task is currently scheduled.
    /// </summary>
    public bool IsScheduled => State is ScheduledState;

    /// <summary>
    ///     Gets whether the task is in draft state.
    /// </summary>
    public bool IsDraft => State is DraftState;

    /// <summary>
    ///     Gets whether the task has failed to schedule.
    /// </summary>
    public bool HasFailed => State is FailedToScheduleState;

    /// <summary>
    ///     Creates a new task item with a new unique identifier.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="dueDate">The due date by which the task must be completed.</param>
    /// <param name="duration">The expected duration of the task.</param>
    /// <param name="priority">The priority level of the task.</param>
    /// <returns>A new TaskItem instance.</returns>
    public static TaskItem Create(
        string name,
        DateTime dueDate,
        TimeSpan duration,
        PriorityLevel priority
    )
    {
        return new TaskItem(name, dueDate, duration, priority);
    }

    /// <summary>
    ///     Creates a task item with an existing identifier, typically used when loading from storage.
    /// </summary>
    /// <param name="name">The name of the task.</param>
    /// <param name="dueDate">The due date by which the task must be completed.</param>
    /// <param name="duration">The expected duration of the task.</param>
    /// <param name="priority">The priority level of the task.</param>
    /// <param name="id">The unique identifier of the existing task.</param>
    /// <returns>A TaskItem instance with the specified identifier.</returns>
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

    /// <summary>
    ///     Gets the current status of the task based on its state.
    /// </summary>
    /// <returns>The current TaskItemStatus.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the task is in an unknown state.</exception>
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

    /// <summary>
    ///     Updates the duration of the task and raises a TaskUpdatedEvent.
    /// </summary>
    /// <param name="newDuration">The new duration for the task.</param>
    /// <exception cref="DomainException">Thrown when the new duration is zero or negative.</exception>
    public void UpdateDuration(TimeSpan newDuration)
    {
        if (newDuration <= TimeSpan.Zero)
            throw new DomainException("Duration must be positive");

        Duration = newDuration;
        AddDomainEvent(new TaskUpdatedEvent(Id, nameof(newDuration), newDuration));
    }

    /// <summary>
    ///     Updates the priority of the task and raises a TaskUpdatedEvent.
    /// </summary>
    /// <param name="newPriority">The new priority level for the task.</param>
    public void UpdatePriority(PriorityLevel newPriority)
    {
        Priority = newPriority;
        AddDomainEvent(new TaskUpdatedEvent(Id, nameof(Priority), newPriority));
    }

    /// <summary>
    ///     Updates the due date of the task and raises a TaskUpdatedEvent.
    /// </summary>
    /// <param name="newDueDate">The new due date for the task.</param>
    /// <exception cref="ArgumentException">Thrown when the new due date is in the past.</exception>
    public void UpdateDueDate(DateTime newDueDate)
    {
        if (newDueDate <= DateTime.Now)
            throw new ArgumentException("Due date must be in the future");

        DueDate = newDueDate;
        AddDomainEvent(new TaskUpdatedEvent(Id, nameof(newDueDate), newDueDate));
    }

    /// <summary>
    ///     Updates the name of the task and raises a TaskUpdatedEvent.
    /// </summary>
    /// <param name="newName">The new name for the task.</param>
    /// <exception cref="ArgumentException">Thrown when the new name is null or whitespace.</exception>
    public void UpdateName(string newName)
    {
        Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
        Name = newName;
        AddDomainEvent(new TaskUpdatedEvent(Id, nameof(Name), newName));
    }

    /// <summary>
    ///     Calculates a score for the task using the provided scoring strategy.
    /// </summary>
    /// <param name="scoringStrategy">The strategy to use for score calculation.</param>
    /// <returns>The calculated score.</returns>
    public int CalculateScore(IScoringStrategy scoringStrategy)
    {
        return scoringStrategy.CalculateScore(this);
    }

    /// <summary>
    ///     Attempts to schedule the task in the specified time window.
    ///     The actual scheduling logic is delegated to the current state.
    /// </summary>
    /// <param name="scheduleTimeWindow">The time window in which to schedule the task.</param>
    public void Schedule(CalendarTimeWindow scheduleTimeWindow)
    {
        State.Schedule(scheduleTimeWindow);
    }

    /// <summary>
    ///     Marks the task as failed to schedule with the specified reason.
    ///     The actual state transition is delegated to the current state.
    /// </summary>
    /// <param name="reason">The reason why the scheduling failed.</param>
    public void MarkAsFailedToSchedule(string reason)
    {
        State.MarkAsFailed(reason);
    }

    /// <summary>
    ///     Attempts to retry scheduling the task.
    ///     The actual retry logic is delegated to the current state.
    /// </summary>
    public void RetryScheduling()
    {
        State.RetryScheduling();
    }

    /// <summary>
    ///     Transitions the task to the scheduled state.
    ///     This method should only be called by state classes.
    /// </summary>
    /// <param name="scheduledTimeWindow">The time window in which the task is scheduled.</param>
    internal void TransitionToScheduled(CalendarTimeWindow scheduledTimeWindow)
    {
        ScheduledTime = scheduledTimeWindow;
        State = new ScheduledState(this);
        AddDomainEvent(new TaskScheduledEvent(Id, scheduledTimeWindow));
    }

    /// <summary>
    ///     Transitions the task to the failed state.
    ///     This method should only be called by state classes.
    /// </summary>
    /// <param name="reason">The reason why the task failed to schedule.</param>
    internal void TransitionToFailed(string reason)
    {
        FailureReason = reason;
        State = new FailedToScheduleState(this);
        AddDomainEvent(new TaskFailedToScheduleEvent(Id, reason));
    }

    /// <summary>
    ///     Transitions the task back to the draft state.
    ///     This method should only be called by state classes.
    /// </summary>
    internal void TransitionToDraft()
    {
        FailureReason = null;
        State = new DraftState(this);
        AddDomainEvent(new TaskSchedulingRetryRequestedEvent(Id));
    }
}
