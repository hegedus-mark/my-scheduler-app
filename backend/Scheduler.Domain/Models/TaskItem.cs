using Scheduler.Domain.Services;
using Scheduler.Shared.Enums;

namespace Scheduler.Domain.Models;

/// <summary>
///     Represents a task to be scheduled, with prioritization and scoring capabilities.
/// </summary>
public class TaskItem : IComparable<TaskItem>
{
    /// <summary>
    ///     Creates a new task with specified parameters.
    /// </summary>
    /// <param name="name">The task name</param>
    /// <param name="dueDate">When the task must be completed</param>
    /// <param name="priorityLevel">The task's priority level</param>
    /// <param name="scoringStrategy">Strategy used to calculate the task's scheduling priority</param>
    /// <param name="duration">Expected time needed to complete the task</param>
    /// <exception cref="ArgumentException">Thrown when duration is not positive</exception>
    public TaskItem(
        string name,
        DateTime dueDate,
        PriorityLevel priorityLevel,
        IScoringStrategy scoringStrategy,
        TimeSpan duration
    )
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        Name = name;
        DueDate = dueDate;
        PriorityLevel = priorityLevel;
        Duration = duration;
        Score = scoringStrategy.CalculateScore(this);
    }

    /// <summary>
    ///     Gets the name of the task.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the due date for task completion.
    /// </summary>
    public DateTime DueDate { get; }

    /// <summary>
    ///     Gets the estimated duration of the task.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///     Gets the priority level of the task.
    /// </summary>
    public PriorityLevel PriorityLevel { get; }

    /// <summary>
    ///     Gets the calculated scheduling priority score.
    /// </summary>
    public int Score { get; }

    /// <summary>
    ///     Compares tasks based on score (descending), due date (ascending),
    ///     priority (descending), and duration (ascending).
    /// </summary>
    public int CompareTo(TaskItem? other)
    {
        if (other == null)
            return 1;

        // Primary sort by score (descending)
        var scoreComparison = other.Score.CompareTo(Score);
        if (scoreComparison != 0)
            return scoreComparison;

        // Secondary sort by due date (ascending)
        var dueDateComparison = DueDate.CompareTo(other.DueDate);
        if (dueDateComparison != 0)
            return dueDateComparison;

        // Tertiary sort by priority level (descending)
        var priorityComparison = other.PriorityLevel.CompareTo(PriorityLevel);
        if (priorityComparison != 0)
            return priorityComparison;

        // Final sort by duration (ascending, shorter tasks first)
        return Duration.CompareTo(other.Duration);
    }
}
