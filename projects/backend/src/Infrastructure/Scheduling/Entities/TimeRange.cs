namespace Infrastructure.Scheduling.Entities;

public class TimeRange
{
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }

    public TimeSpan ToTimeSpan()
    {
        return TimeSpan.FromDays(Days) + TimeSpan.FromHours(Hours) + TimeSpan.FromMinutes(Minutes);
    }
}

public static class TimeSpanExtensions
{
    public static TimeRange ToTimeRange(this TimeSpan timespan)
    {
        return new TimeRange
        {
            Days = timespan.Days,
            Hours = timespan.Hours,
            Minutes = timespan.Minutes,
        };
    }
}
