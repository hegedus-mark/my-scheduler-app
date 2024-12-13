namespace Scheduler.Domain.Shared;

/// <summary>
///     Represents an immutable time period with a start and end time.
/// </summary>
public readonly record struct TimeSlot
{
    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(1);

    private TimeSlot(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    ///     Gets the start time of the slot.
    /// </summary>
    public TimeOnly Start { get; }

    /// <summary>
    ///     Gets the end time of the slot.
    /// </summary>
    public TimeOnly End { get; }

    /// <summary>
    ///     Gets the duration of the time slot.
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    ///     Creates a new time slot with the specified start and end times.
    /// </summary>
    /// <param name="start">The start time</param>
    /// <param name="end">The end time</param>
    /// <returns>A new TimeSlot instance</returns>
    /// <exception cref="ArgumentException">Thrown when start time is not before end time or duration is less than one minute</exception>
    public static TimeSlot Create(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time.");

        if (end - start < MinDuration)
            throw new ArgumentException("Duration must be at least 1 minute.");

        return new TimeSlot(start, end);
    }

    /// <summary>
    ///     Checks if this time slot fully contains another time slot.
    /// </summary>
    /// <param name="other">The time slot to check</param>
    /// <returns>True if this slot contains the other slot completely</returns>
    public bool Contains(TimeSlot other)
    {
        return Start <= other.Start && End >= other.End;
    }

    /// <summary>
    ///     Checks if this time slot overlaps with another time slot.
    /// </summary>
    /// <param name="other">The time slot to check</param>
    /// <returns>True if the slots overlap in any way</returns>
    public bool Overlaps(TimeSlot other)
    {
        return Start < other.End && End > other.Start;
    }

    public override string ToString()
    {
        return $"{Start:HH:mm} - {End:HH:mm}";
    }
}
