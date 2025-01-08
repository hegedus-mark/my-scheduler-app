using SharedKernel.Extensions;

namespace Domain.Shared.ValueObjects;

public readonly record struct DateRange
{
    public DateRange(DateOnly start, DateOnly end)
    {
        if (end < start)
            throw new ArgumentException("End date must be on or after start date");

        Start = start;
        End = end;
    }

    public DateOnly Start { get; }
    public DateOnly End { get; }

    public TimeSpan Duration => End.ToDateTime().Subtract(Start.ToDateTime());

    public static DateRange CreateFromDateTimes(DateTime start, DateTime end)
    {
        return new DateRange(start.ToDateOnly(), end.ToDateOnly());
    }

    public static DateRange CreateFromDuration(DateOnly start, TimeSpan duration)
    {
        if (duration.TotalDays < 0)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        return new DateRange(start, start.ToDateTime().Add(duration).ToDateOnly());
    }

    public static DateRange CreateFromDuration(
        DateOnly start,
        int years = 0,
        int months = 0,
        int days = 0
    )
    {
        if (years == 0 && months == 0 && days == 0)
            throw new ArgumentException("Duration must be greater than 0");

        var endDate = start.AddYears(years).AddMonths(months).AddDays(days);

        return new DateRange(start, endDate);
    }

    public static DateRange CreateForWeek(DateOnly start)
    {
        return new DateRange(start, start.AddDays(6));
    }

    public static DateRange CreateForMonth(int year, int month)
    {
        return new DateRange(
            new DateOnly(year, month, 1),
            new DateOnly(year, month, DateTime.DaysInMonth(year, month))
        );
    }

    public IEnumerable<DateOnly> GetDates()
    {
        for (var date = Start; date <= End; date = date.AddDays(1))
            yield return date;
    }

    public (DateRange?, DateRange?) Split(DateOnly at)
    {
        if (!Contains(at))
            return (null, null);

        if (at == Start || at == End) //Could throw an error or send back the same dateRange, haven't figured it out yet
            return (null, null);

        return (new DateRange(Start, at.AddDays(-1)), new DateRange(at, End));
    }

    public bool Contains(DateOnly date)
    {
        return date >= Start && date <= End;
    }

    public bool Contains(DateTime date)
    {
        return date.ToDateOnly() >= Start && date.ToDateOnly() <= End;
    }

    public bool Contains(DateRange other)
    {
        return other.Start >= Start && other.End <= End;
    }

    public bool Overlaps(DateRange other)
    {
        return Start <= other.End && End >= other.Start;
    }

    public DateRange? Intersect(DateRange other)
    {
        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return start <= end ? new DateRange(start, end) : null;
    }

    public DateRange Union(DateRange other)
    {
        if (!Overlaps(other))
            throw new ArgumentException("Cannot union non-overlapping ranges");

        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateRange(start, end);
    }
}
