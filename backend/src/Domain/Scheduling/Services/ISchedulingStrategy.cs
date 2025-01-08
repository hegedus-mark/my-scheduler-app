using Domain.Scheduling.Models;
using Domain.Scheduling.Results;
using CalendarTimeWindow = Domain.Shared.ValueObjects.CalendarTimeWindow;

namespace Domain.Scheduling.Services;

public interface ISchedulingStrategy
{
    SchedulingResult Schedule(
        IReadOnlyCollection<TaskItem> tasks,
        IReadOnlyList<CalendarTimeWindow> availableWindows
    );
}
