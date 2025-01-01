using Scheduling.Domain.Models;

namespace Scheduling.Domain.Services;

public interface IScoringStrategy
{
    int CalculateScore(TaskItem taskItem);
}
