namespace Scheduler.Core.Models;

public readonly record struct TimeSlot
{
    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(1);

    private TimeSlot(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeSpan Duration => End - Start;

    public static TimeSlot Create(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time.");

        if (end - start < MinDuration)
            throw new ArgumentException("Duration must be at least 1 minute.");

        return new TimeSlot(start, end);
    }

    public bool Contains(TimeSlot other)
    {
        return Start <= other.Start && End >= other.End;
    }

    public bool Overlaps(TimeSlot other)
    {
        return Start < other.End && End > other.Start;
    }

    public override string ToString()
    {
        return $"{Start:HH:mm} - {End:HH:mm}";
    }
}
