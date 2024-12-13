using Scheduler.Domain.Shared;

namespace Scheduler.Domain.Calendars.Configuration;

public class DayScheduleOverride
{
    public DayScheduleOverride(
        DateOnly date,
        TimeSlot? customWorkingHours = null,
        bool? isWorkingDay = null,
        string? overrideReason = null
    )
    {
        Date = date;
        CustomWorkingHours = customWorkingHours;
        IsWorkingDay = isWorkingDay;
        OverrideReason = overrideReason;
    }

    public TimeSlot? CustomWorkingHours { get; }
    public DateOnly Date { get; }

    public bool? IsWorkingDay { get; }
    public string? OverrideReason { get; }

    public static DayScheduleOverride CreateWorkingHoursOverride(
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string? reason = null
    )
    {
        var timeSlot = TimeSlot.Create(startTime, endTime);
        return new DayScheduleOverride(date, timeSlot, null, reason);
    }

    public static DayScheduleOverride CreateWorkingDayOverride(
        DateOnly date,
        bool isWorkingDay,
        string? reason = null
    )
    {
        return new DayScheduleOverride(date, null, isWorkingDay, reason);
    }

    public bool ModifiesSchedule(UserScheduleConfig defaultConfig)
    {
        if (IsWorkingDay.HasValue)
            return true;

        if (CustomWorkingHours.HasValue)
        {
            var defaultStart = defaultConfig.DefaultWorkStartTime;
            var defaultEnd = defaultConfig.DefaultWorkEndTime;

            return CustomWorkingHours.Value.Start != defaultStart
                || CustomWorkingHours.Value.End != defaultEnd;
        }

        return false;
    }
}
