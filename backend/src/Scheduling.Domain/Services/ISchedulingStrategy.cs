using Scheduling.Domain.Models;
using Scheduling.Domain.Results;
using SharedKernel.Domain.ValueObjects;

namespace Scheduling.Domain.Services;

public interface ISchedulingStrategy
{
    SchedulingResult Schedule(
        IReadOnlyCollection<TaskItem> tasks,
        IReadOnlyList<CalendarTimeWindow> availableWindows
    );
}
