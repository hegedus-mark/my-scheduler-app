using Domain.Scheduling.Models;
using Domain.Scheduling.Results;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Scheduling.Services;

public interface ISchedulingStrategy
{
    SchedulingResult Schedule(
        IReadOnlyCollection<TaskItem> tasks,
        IReadOnlyList<CalendarTimeWindow> availableWindows
    );
}
