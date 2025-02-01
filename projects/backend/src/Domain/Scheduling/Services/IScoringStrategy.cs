using Domain.Scheduling.Models;

namespace Domain.Scheduling.Services;

public interface IScoringStrategy
{
    int CalculateScore(TaskItem taskItem);
}
