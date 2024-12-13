using Scheduler.Domain.Calendars.Configuration;
using Scheduler.Domain.Shared.Enums;
using Scheduler.Domain.Tasks.Entities;

namespace Scheduler.Domain.Tasks.Scoring;

public class SimpleScoringStrategy : IScoringStrategy
{
    private readonly UserConfig _userConfig;

    public SimpleScoringStrategy(UserConfig userConfig)
    {
        _userConfig = userConfig;
    }

    public int CalculateScore(TaskItem taskItem)
    {
        var score = 0;

        var timeUntilDue = taskItem.DueDate - DateTime.Today;
        score += (int)(100 / (timeUntilDue.TotalDays + 1)); //Just some random formula to score due dates

        if (_userConfig.LongestJobFirst)
            score += taskItem.Duration.Hours * 10;
        else
            score -= taskItem.Duration.Hours * 10;

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
