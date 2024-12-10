namespace Scheduler.Core.Models;

public readonly struct TimeSlot : IEquatable<TimeSlot>
{
    public readonly DateTime Start { get; }
    public readonly DateTime End { get; }

    public TimeSpan Duration => End - Start;
    
    //Start and end is on the same day (So start should be enough)
    public DateTime Day => Start.Date; 

    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(1);

    private TimeSlot(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static TimeSlot Create(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time.");

        if (start.Date != end.Date)
            throw new ArgumentException("Start and end times must be on the same day.");

        if (end - start < MinDuration)
            throw new ArgumentException("Duration must be at least 1 minute.");

        return new TimeSlot(start, end);
    }

    public bool IsInsideTimeSlot(TimeSlot other)
    {
        return other.Start <= this.Start && other.End >= this.End;
    }

    public bool Equals(TimeSlot other)
    {
        return Start == other.Start && End == other.End;
    }

    public override bool Equals(object? obj)
    {
        return obj is TimeSlot other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    public static bool operator ==(TimeSlot left, TimeSlot right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TimeSlot left, TimeSlot right)
    {
        return !left.Equals(right);
    }

    // Nice string representation for debugging
    public override string ToString()
    {
        return $"{Start:HH:mm} - {End:HH:mm}";
    }

    // Helper methods that make working with TimeSlots more convenient
    public bool Overlaps(TimeSlot other)
    {
        return Start < other.End && other.Start < End;
    }

    public bool Contains(DateTime time)
    {
        return time >= Start && time <= End;
    }
}