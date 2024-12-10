namespace Scheduler.Core.Models.Scoring;

public interface IScoringStrategy
{
    int CalculateScore(UnscheduledTask task);
}