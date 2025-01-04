using Domain.Scheduling.Models;
using Domain.Scheduling.Models.Enums;

namespace Domain.Scheduling.Services;

public class SimpleScoringStrategy : IScoringStrategy
{
    public int CalculateScore(TaskItem taskItem)
    {
        var score = 0;

        var timeUntilDue = taskItem.DueDate - DateTime.Today;
        score += (int)(100 / (timeUntilDue.TotalDays + 1)); //Just some random formula to score due dates

        score += taskItem.Duration.Hours * 10;

        score += taskItem.Priority switch
        {
            PriorityLevel.High => 50,
            PriorityLevel.Medium => 25,
            PriorityLevel.Low => 10,
            _ => throw new ArgumentOutOfRangeException(),
        };

        return score;
    }
}
