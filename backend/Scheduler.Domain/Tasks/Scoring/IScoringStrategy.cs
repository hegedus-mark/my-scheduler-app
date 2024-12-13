using Scheduler.Domain.Tasks.Entities;

namespace Scheduler.Domain.Tasks.Scoring;

public interface IScoringStrategy
{
    int CalculateScore(TaskItem taskItem);
}
