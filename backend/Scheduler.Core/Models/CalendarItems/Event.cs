using Scheduler.Core.Models.CalendarItems;

namespace Scheduler.Core.Models;

public class Event : CalendarItem
{
    public bool IsRecurring { get; set; }

   
    // Constructor for new events
    public Event(TimeSlot timeSlot, bool isRecurring)
        : base(timeSlot)
    {
        IsRecurring = isRecurring;
    }

    // Constructor for existing events
    public Event(Guid id, TimeSlot timeSlot, bool isRecurring)
        : base(id, timeSlot)
    {
        IsRecurring = isRecurring;
    }
}