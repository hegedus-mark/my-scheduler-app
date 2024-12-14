using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Shared.Results;

namespace Scheduler.Domain.Services;

public interface ISchedulingStrategy
{
    SchedulingResult Schedule(
        IReadOnlyList<WorkingDay> availableDays,
        IReadOnlyCollection<TaskItem> tasks,
        UserScheduleConfig config
    );
}
