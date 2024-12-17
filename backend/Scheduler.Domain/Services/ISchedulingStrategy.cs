using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Results;

namespace Scheduler.Domain.Services;

public interface ISchedulingStrategy
{
    SchedulingResult Schedule(
        IReadOnlyList<WorkingDay> availableDays,
        IReadOnlyCollection<TaskItem> tasks
    );
}
