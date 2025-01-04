using Domain.Calendar.Models.Enums;

namespace Domain.Calendar.Models.Configuration;

public class UserScheduleConfig
{
    public UserScheduleConfig(
        TimeOnly defaultWorkStartTime,
        TimeOnly defaultWorkEndTime,
        DaysOfWeek workingDays,
        TimeSpan minimumTaskDuration
    )
    {
        // Validate work hours
        if (defaultWorkStartTime >= defaultWorkEndTime)
            throw new ArgumentException("Work end time must be after start time");

        // Validate minimum durations
        if (minimumTaskDuration <= TimeSpan.Zero)
            throw new ArgumentException("Minimum task duration must be positive");

        DefaultWorkStartTime = defaultWorkStartTime;
        DefaultWorkEndTime = defaultWorkEndTime;
        WorkingDays = workingDays;
        MinimumTaskDuration = minimumTaskDuration;
    }

    public TimeOnly DefaultWorkStartTime { get; }
    public TimeOnly DefaultWorkEndTime { get; }
    public DaysOfWeek WorkingDays { get; }
    public TimeSpan MinimumTaskDuration { get; }

    // Factory method for default settings now uses the constructor
    public static UserScheduleConfig CreateDefault()
    {
        return new UserScheduleConfig(
            new TimeOnly(9, 0),
            new TimeOnly(17, 0),
            DaysOfWeek.Monday
                | DaysOfWeek.Tuesday
                | DaysOfWeek.Wednesday
                | DaysOfWeek.Thursday
                | DaysOfWeek.Friday,
            TimeSpan.FromMinutes(30)
        );
    }
}
