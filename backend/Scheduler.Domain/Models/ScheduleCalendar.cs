using Scheduler.Domain.Calendars.Interfaces;
using Scheduler.Domain.Extensions;
using Scheduler.Domain.Models.Base;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared;
using Scheduler.Domain.Shared.Results;
using Scheduler.Domain.ValueObjects;

namespace Scheduler.Domain.Models;

public class ScheduleCalendar : EntityBase
{
    private const int DATE_RANGE_TO_BE_INITIALISED = 365; //Initialise the first year
    private readonly SortedDictionary<DateOnly, ICalendarDay> _days;
    private readonly SortedDictionary<DateOnly, DayScheduleOverride> _dayScheduleOverrides;
    private readonly UserTaskScheduler _taskScheduler;
    private readonly UserScheduleConfig _userScheduleConfig;

    private ScheduleCalendar(
        Guid? id,
        UserScheduleConfig userScheduleConfig,
        UserTaskScheduler taskScheduler
    )
        : base(id)
    {
        var initialDateRange = DateRange.CreateFromDuration(
            DateTime.Now.ToDateOnly(),
            0,
            0,
            DATE_RANGE_TO_BE_INITIALISED
        );
        _days = new SortedDictionary<DateOnly, ICalendarDay>();
        _dayScheduleOverrides = new SortedDictionary<DateOnly, DayScheduleOverride>();
        EnsureDaysExist(initialDateRange);
        _userScheduleConfig = userScheduleConfig;
        _taskScheduler = taskScheduler;
    }

    public static ScheduleCalendar Create(
        UserScheduleConfig userScheduleConfig,
        UserTaskScheduler userTaskScheduler
    )
    {
        return new ScheduleCalendar(null, userScheduleConfig, userTaskScheduler);
    }

    public static ScheduleCalendar Load(
        Guid id,
        UserScheduleConfig userScheduleConfig,
        UserTaskScheduler userTaskScheduler
    )
    {
        return new ScheduleCalendar(null, userScheduleConfig, userTaskScheduler);
    }

    public IEnumerable<ICalendarDay> GetDaysInRage(DateOnly start, DateOnly end)
    {
        return _days.Where(kv => kv.Key >= start && kv.Key <= end).Select(kv => kv.Value);
    }

    public IEnumerable<WorkingDay> GetWorkingDaysInRange(DateRange dateRange)
    {
        return _days
            .Where(kv => kv.Key >= dateRange.Start && kv.Key <= dateRange.End)
            .Select(kv => kv.Value)
            .OfType<WorkingDay>();
    }

    public SchedulingResult ScheduleTasks(
        IReadOnlyCollection<TaskItem> taskToBeScheduled,
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

        EnsureDaysExist(schedulingWindow.Value);
        var result = _taskScheduler.ScheduleTasks(
            GetWorkingDaysInRange(schedulingWindow.Value).ToList(),
            taskToBeScheduled
        );
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
                {
                    if (overrideConfig.IsWorkingDay)
                    {
                        var start =
                            overrideConfig.CustomWorkingHours?.Start
                            ?? throw new InvalidOperationException(
                                "overrideDay config must have an start Time"
                            );
                        var end =
                            overrideConfig.CustomWorkingHours?.End
                            ?? throw new InvalidOperationException(
                                "overrideDay config must have an end Time"
                            );

                        timeSlot = TimeSlot.Create(start, end);
                    }
                    else
                    {
                        _days.Add(requestedDate, NonWorkingDay.Create(requestedDate));
                        return;
                    }
                }

                _days.Add(requestedDate, WorkingDay.Create(requestedDate, timeSlot));
            }
    }
}
