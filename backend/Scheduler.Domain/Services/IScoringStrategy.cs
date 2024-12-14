using Scheduler.Domain.Models;

namespace Scheduler.Domain.Services;

public interface IScoringStrategy
{
    int CalculateScore(TaskItem taskItem);
}
