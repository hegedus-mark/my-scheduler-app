using Scheduler.Domain.Models.Interfaces;
using Scheduler.Domain.Models.Results;
using Scheduler.Domain.Services;

namespace Scheduler.Domain.Models;

public class ScheduleWindow
{
    private readonly Dictionary<DateOnly, ICalendarDay> _days;
    private readonly ISchedulingStrategy _schedulingStrategy;

    public ScheduleWindow(IEnumerable<ICalendarDay> days, ISchedulingStrategy schedulingStrategy)
    {
        _days = days.ToDictionary(d => d.DayDate);
        _schedulingStrategy = schedulingStrategy;
    }

    public SchedulingResult ScheduleTasks(IReadOnlyCollection<TaskItem> tasksToBeScheduled)
    {
        var workingDays = _days.Values.OfType<WorkingDay>().ToList();
        return _schedulingStrategy.Schedule(workingDays, tasksToBeScheduled);
    }
}
