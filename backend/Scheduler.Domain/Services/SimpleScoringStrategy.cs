using Scheduler.Domain.Models;
using Scheduler.Shared.Enums;

namespace Scheduler.Domain.Services;

public class SimpleScoringStrategy : IScoringStrategy
{
    public int CalculateScore(TaskItem taskItem)
    {
        var score = 0;

        var timeUntilDue = taskItem.DueDate - DateTime.Today;
        score += (int)(100 / (timeUntilDue.TotalDays + 1)); //Just some random formula to score due dates

        score += taskItem.Duration.Hours * 10;

        score += taskItem.PriorityLevel switch
        {
            PriorityLevel.High => 50,
            PriorityLevel.Medium => 25,
            PriorityLevel.Low => 10,
            _ => throw new ArgumentOutOfRangeException(),
        };

        return score;
    }
}
