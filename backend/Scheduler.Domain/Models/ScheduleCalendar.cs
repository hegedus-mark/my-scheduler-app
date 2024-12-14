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
    private const int DATE_RANGE_TO_BE_INITIALISED = 365; //TODO: Put this in appsettings
    private readonly SortedDictionary<DateOnly, ICalendarDay> _days;
    private readonly SortedDictionary<DateOnly, DayScheduleOverride> _dayScheduleOverrides;
    private readonly ISchedulingStrategy _schedulingStrategy;
    private readonly UserScheduleConfig _userScheduleConfig;

    private ScheduleCalendar(
        Guid? id,
        UserScheduleConfig userScheduleConfig,
        ISchedulingStrategy schedulingStrategy
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
        _schedulingStrategy = schedulingStrategy;
    }

    public static ScheduleCalendar Create(
        UserScheduleConfig userScheduleConfig,
        ISchedulingStrategy schedulingStrategy
    )
    {
        return new ScheduleCalendar(null, userScheduleConfig, schedulingStrategy);
    }

    public static ScheduleCalendar Load(
        Guid id,
        UserScheduleConfig userScheduleConfig,
        ISchedulingStrategy schedulingStrategy
    )
    {
        return new ScheduleCalendar(null, userScheduleConfig, schedulingStrategy);
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
        IReadOnlyCollection<TaskItem> tasksToBeScheduled,
        DateRange? schedulingWindow = null
    )
    {
        var window = DetermineSchedulingWindow(tasksToBeScheduled, schedulingWindow);
        EnsureDaysExist(window);

        var workingDays = GetWorkingDaysInRange(window).ToList();
        return _schedulingStrategy.Schedule(workingDays, tasksToBeScheduled, _userScheduleConfig);
    }

    private DateRange DetermineSchedulingWindow(
        IReadOnlyCollection<TaskItem> tasks,
        DateRange? schedulingWindow
    )
    {
        if (schedulingWindow.HasValue)
            return schedulingWindow.Value;

        var tomorrow = DateTime.Now.AddDays(1).ToDateOnly();
        var lastTaskDueDate =
            tasks.MaxBy(t => t.DueDate)?.DueDate
            ?? throw new InvalidOperationException("No tasks provided with due dates");

        return new DateRange(tomorrow, lastTaskDueDate.ToDateOnly());
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
