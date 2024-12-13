using Scheduler.Core.Algo;
using Scheduler.Core.Extensions;
using Scheduler.Core.Models.CalendarItems;
using Scheduler.Core.Models.Results;
using Scheduler.Core.Models.UserConfigs;

namespace Scheduler.Core.Models;

public class ScheduleCalendar : EntityBase
{
    private readonly SortedDictionary<DateOnly, ICalendarDay> _days;
    private readonly SortedDictionary<DateOnly, DayScheduleOverride> _dayScheduleOverrides;
    private readonly UserTaskScheduler _taskScheduler;
    private readonly UserScheduleConfig _userScheduleConfig;

    //new Calendar
    public ScheduleCalendar()
        : base(Guid.NewGuid()) { }

    //existing Calendar
    public ScheduleCalendar(Guid id)
        : base(id) { }

    public IEnumerable<ICalendarDay> GetDaysInRage(DateOnly start, DateOnly end)
    {
        return _days.Where(kv => kv.Key >= start && kv.Key <= end).Select(kv => kv.Value);
    }

    public SchedulingResult ScheduleTasks(
        IEnumerable<TaskItem> taskToBeScheduled,
        DateRange? schedulingWindow = null
    )
    {
        //if no schedulingWindow use the latest dueDate
        if (!schedulingWindow.HasValue)
        {
            var tomorrow = DateTime.Now.AddDays(1).ToDateOnly();
            var lastTaskDueDate =
                taskToBeScheduled.MaxBy(t => t.DueDate)?.DueDate
                ?? throw new InvalidOperationException("There wasn't any DueDate in taskItem");
            schedulingWindow = new DateRange(tomorrow, lastTaskDueDate.ToDateOnly());
        }

        //var result = _taskScheduler.ScheduleTasks(taskToBeScheduled);
        throw new NotImplementedException();
    }

    private void EnsureDaysExist(DateRange dateWindow)
    {
        var requestedDates = dateWindow.GetDates();

        foreach (var requestedDate in requestedDates)
            if (!_days.ContainsKey(requestedDate))
            {
                var timeSlot = TimeSlot.Create(
                    _userScheduleConfig.DefaultWorkStartTime,
                    _userScheduleConfig.DefaultWorkEndTime
                );
                if (_dayScheduleOverrides.TryGetValue(requestedDate, out var overrideConfig))
                    timeSlot =
                        overrideConfig.IsWorkingDay.HasValue && overrideConfig.IsWorkingDay.Value
                            ? TimeSlot.Create(
                                overrideConfig.CustomWorkingHours.Value.Start,
                                overrideConfig.CustomWorkingHours.Value.End
                            )
                            : TimeSlot.Create(TimeOnly.MinValue, TimeOnly.MinValue);

                _days.Add(requestedDate, WorkingDay.Create(requestedDate, timeSlot));
            }
    }
}
