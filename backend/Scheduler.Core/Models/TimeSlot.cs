using Scheduler.Core.Extensions;

namespace Scheduler.Core.Models;

public readonly record struct TimeSlot
{
    public DateOnly Day { get; }
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeSpan Duration => End - Start;

    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(1);

    private TimeSlot(DateOnly day, TimeOnly start, TimeOnly end)
    {
        Day = day;
        Start = start;
        End = end;
    }

    public static TimeSlot Create(DateOnly day, TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time.");

        if (end - start < MinDuration)
            throw new ArgumentException("Duration must be at least 1 minute.");

        return new TimeSlot(day, start, end);
    }


    public bool IsInsideTimeSlot(TimeSlot other)
    {
        return Day == other.Day &&
               other.Start <= Start &&
               other.End >= End;
    }

    public override string ToString() => $"{Start:HH:mm} - {End:HH:mm}";

    public bool Overlaps(TimeSlot other)
    {
        return Day == other.Day &&
               Start < other.End &&
               other.Start < End;
    }

    public bool Contains(DateTime time)
    {
        var timeDate = DateOnly.FromDateTime(time);
        var timeOfDay = TimeOnly.FromDateTime(time);

        return Day == timeDate &&
               timeOfDay >= Start &&
               timeOfDay <= End;
    }

    public bool IsBefore(DateTime dateTime)
    {
        var compareDate = dateTime.ToDateOnly();
        var compareTime = dateTime.ToTimeOnly();

        return Day < compareDate ||
               (Day == compareDate && End <= compareTime);
    }

    public bool IsAfter(DateTime dateTime)
    {
        var compareDate = dateTime.ToDateOnly();
        var compareTime = dateTime.ToTimeOnly();

        return Day > compareDate ||
               (Day == compareDate && Start >= compareTime);
    }
}