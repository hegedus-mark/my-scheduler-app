using Domain.Shared.ValueObjects;

namespace Domain.Scheduling.Models.TaskStates;

/// <summary>
///     Represents the base abstract class for the Task state pattern implementation.
///     This class defines the contract for different states a task can be in and
///     the allowed operations for each state.
/// </summary>
/// <remarks>
///     The TaskState class is part of the State pattern implementation where each
///     concrete state class handles the task's behavior differently based on its
///     current state. This pattern ensures that tasks can only transition between
///     states in valid ways and maintain consistent behavior.
/// </remarks>
public abstract class TaskState
{
    /// <summary>
    ///     Gets the task instance that this state belongs to.
    /// </summary>
    /// <remarks>
    ///     Protected access ensures that only derived state classes can access the task
    ///     to modify its state.
    /// </remarks>
    protected readonly TaskItem Task;

    /// <summary>
    ///     Initializes a new instance of a TaskState with the associated task.
    /// </summary>
    /// <param name="task">The task instance that this state will manage.</param>
    /// <remarks>
    ///     Each concrete state implementation must provide the task instance
    ///     that it will manage through this constructor.
    /// </remarks>
    protected TaskState(TaskItem task)
    {
        Task = task;
    }

    /// <summary>
    ///     Attempts to schedule the task in the specified time window.
    /// </summary>
    /// <param name="scheduleTimeWindow">The time window in which to schedule the task.</param>
    /// <remarks>
    ///     Each concrete state implementation must define whether scheduling is allowed
    ///     in that state and handle the scheduling attempt appropriately. Some states
    ///     may throw exceptions if scheduling is not allowed.
    /// </remarks>
    public abstract void Schedule(CalendarTimeWindow scheduleTimeWindow);

    /// <summary>
    ///     Marks the task as failed with the specified reason.
    /// </summary>
    /// <param name="reason">The reason explaining why the task failed.</param>
    /// <remarks>
    ///     Each concrete state implementation must define whether marking as failed is allowed
    ///     in that state and handle the failure appropriately. Some states may throw
    ///     exceptions if marking as failed is not allowed.
    /// </remarks>
    public abstract void MarkAsFailed(string reason);

    /// <summary>
    ///     Attempts to retry scheduling the task.
    /// </summary>
    /// <remarks>
    ///     Each concrete state implementation must define whether retrying is allowed
    ///     in that state and handle the retry attempt appropriately. Some states may
    ///     throw exceptions if retrying is not allowed.
    /// </remarks>
    public abstract void RetryScheduling();
}
