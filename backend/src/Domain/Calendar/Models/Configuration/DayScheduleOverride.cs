using SharedKernel.Domain.ValueObjects;

namespace Domain.Calendar.Models.Configuration;

public class DayScheduleOverride
{
    private DayScheduleOverride(
        DateOnly date,
        bool isWorkingDay,
        TimeSlot? customWorkingHours = null,
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

    public bool IsWorkingDay { get; }
    public string? OverrideReason { get; }

    public static DayScheduleOverride CreateWorkingHoursOverride(
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string? reason = null
    )
    {
        var timeSlot = TimeSlot.Create(startTime, endTime);
        return new DayScheduleOverride(date, true, timeSlot, reason);
    }

    public static DayScheduleOverride CreateWorkingDayOverride(
        DateOnly date,
        bool isWorkingDay,
        string? reason = null
    )
    {
        return new DayScheduleOverride(date, isWorkingDay, null, reason);
    }

    public bool ModifiesSchedule(UserScheduleConfig defaultConfig)
    {
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
