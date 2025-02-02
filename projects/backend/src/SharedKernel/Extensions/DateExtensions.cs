namespace SharedKernel.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MinValue);
    }

    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return TimeOnly.FromDateTime(dateTime);
    }

    public static DateTime ToDateTime(this TimeOnly timeOnly, DateOnly date)
    {
        return new DateTime(date, timeOnly);
    }

    public static bool IsSameDay(this DateOnly date, DateTime dateTime)
    {
        return date == DateOnly.FromDateTime(dateTime);
    }

    public static bool IsAfterDay(this DateOnly date, DateTime dateTime)
    {
        return date > DateOnly.FromDateTime(dateTime);
    }

    public static bool IsBeforeDay(this DateOnly date, DateTime dateTime)
    {
        return date < DateOnly.FromDateTime(dateTime);
    }

    public static bool IsOnOrBeforeDay(this DateOnly date, DateTime dateTime)
    {
        return date <= DateOnly.FromDateTime(dateTime);
    }

    public static bool IsOnOrAfterDay(this DateOnly date, DateTime dateTime)
    {
        return date >= DateOnly.FromDateTime(dateTime);
    }
}
