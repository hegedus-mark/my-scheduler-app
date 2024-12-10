using Scheduler.Core.Enum;
using Scheduler.Core.Models.Scoring;

namespace Scheduler.Core.Models;

public class UnscheduledTask : IComparable<UnscheduledTask>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DueDate { get; set; }
    public TimeSpan Duration { get; set; }

    private readonly IScoringStrategy _scoringStrategy;
    public PriorityLevel PriorityLevel { get; set; }
    public int Score { get; set; }

    public UnscheduledTask(int id,
        string name,
        DateTime dueDate,
        PriorityLevel priorityLevel,
        IScoringStrategy scoringStrategy, TimeSpan duration)
    {
        
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));
        
        Id = id;
        Name = name;
        DueDate = dueDate;
        PriorityLevel = priorityLevel;
        _scoringStrategy = scoringStrategy;
        Duration = duration;
        Score = _scoringStrategy.CalculateScore(this);
    }

    public int CompareTo(UnscheduledTask? other)
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