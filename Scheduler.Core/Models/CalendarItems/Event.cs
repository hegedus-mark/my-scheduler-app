namespace Scheduler.Core.Models;

public class Event : CalendarItem
{
    public bool IsRecurring { get; set; }

    public Event(TimeSlot timeSlot, bool isRecurring) : base(timeSlot)
    {
        IsRecurring = isRecurring;
    }
}