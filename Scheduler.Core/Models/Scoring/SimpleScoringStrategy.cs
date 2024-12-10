using Scheduler.Core.Enum;

namespace Scheduler.Core.Models.Scoring;

public class SimpleScoringStrategy : IScoringStrategy
{
    private readonly UserConfig _userConfig;

    public SimpleScoringStrategy(UserConfig userConfig)
    {
        _userConfig = userConfig;
    }
    
    public int CalculateScore(TaskItem taskItem)
    {
        int score = 0;

        TimeSpan timeUntilDue = taskItem.DueDate - DateTime.Today;
        score += (int)(100 / (timeUntilDue.TotalDays + 1)); //Just some random formula to score due dates

        if (_userConfig.LongestJobFirst)
        {
            score += (int)taskItem.Duration.Hours * 10;
        }
        else
        {
            score -= (int)taskItem.Duration.Hours * 10;
        }

        score += taskItem.PriorityLevel switch
        {
            PriorityLevel.High => 50,
            PriorityLevel.Medium => 25,
            PriorityLevel.Low => 10,
            _ => throw new ArgumentOutOfRangeException()
        };

        return score;
    }
}