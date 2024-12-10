using Scheduler.Core.Enum;
using Scheduler.Core.Models.Scoring;

namespace Scheduler.Core.Models;

public class TaskItem : IComparable<TaskItem>
{
    public string Name { get; }
    public DateTime DueDate { get; }
    public TimeSpan Duration { get; }
    public PriorityLevel PriorityLevel { get; }
    public int Score { get; }

    public TaskItem(
        string name,
        DateTime dueDate,
        PriorityLevel priorityLevel,
        IScoringStrategy scoringStrategy, TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        Name = name;
        DueDate = dueDate;
        PriorityLevel = priorityLevel;
        Duration = duration;
        Score = scoringStrategy.CalculateScore(this);
    }

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