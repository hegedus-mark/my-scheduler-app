namespace SharedKernel.Domain.ValueObjects;

public readonly record struct CalendarTimeWindow
{
    private CalendarTimeWindow(DateOnly date, TimeSlot timeSlot)
    {
        Date = date;
        TimeSlot = timeSlot;
    }

    public DateOnly Date { get; }
    public TimeSlot TimeSlot { get; }

    public DateTime EndDate => Date.ToDateTime(TimeSlot.End);
    public DateTime StartDate => Date.ToDateTime(TimeSlot.Start);

    public static CalendarTimeWindow Create(DateOnly date, TimeSlot timeSlot)
    {
        return new CalendarTimeWindow(date, timeSlot);
    }

    public bool CoversTime(CalendarTimeWindow other)
    {
        return Date == other.Date && TimeSlot.Overlaps(other.TimeSlot);
    }

    // Helper method to check if this window can accommodate a duration
    public bool CanAccommodate(TimeSpan duration)
    {
        return TimeSlot.Duration >= duration;
    }
}
