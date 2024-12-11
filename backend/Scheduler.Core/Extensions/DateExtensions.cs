namespace Scheduler.Core.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);

    public static DateTime ToDateTime(this DateOnly dateOnly) =>
        dateOnly.ToDateTime(TimeOnly.MinValue);

    public static TimeOnly ToTimeOnly(this DateTime dateTime) => TimeOnly.FromDateTime(dateTime);

    public static bool IsSameDay(this DateOnly date, DateTime dateTime) =>
        date == DateOnly.FromDateTime(dateTime);

    public static bool IsAfterDay(this DateOnly date, DateTime dateTime) =>
        date > DateOnly.FromDateTime(dateTime);

    public static bool IsBeforeDay(this DateOnly date, DateTime dateTime) =>
        date < DateOnly.FromDateTime(dateTime);

    public static bool IsOnOrBeforeDay(this DateOnly date, DateTime dateTime) =>
        date <= DateOnly.FromDateTime(dateTime);

    public static bool IsOnOrAfterDay(this DateOnly date, DateTime dateTime) =>
        date >= DateOnly.FromDateTime(dateTime);
}
