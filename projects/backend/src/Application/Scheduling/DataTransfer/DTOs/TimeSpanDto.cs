namespace Application.Scheduling.DataTransfer.DTOs;

public record TimeSpanDto(int Days = 0, int Hours = 0, int Minutes = 0, int Seconds = 0)
{
    /// <summary>
    ///     Converts the TimeSpanDto to a System.TimeSpan
    /// </summary>
    public TimeSpan ToTimeSpan()
    {
        return new TimeSpan(Days, Hours, Minutes, Seconds, 0);
    }

    /// <summary>
    ///     Creates a TimeSpanDto from a System.TimeSpan
    /// </summary>
    public static TimeSpanDto FromTimeSpan(TimeSpan timeSpan)
    {
        return new TimeSpanDto(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
