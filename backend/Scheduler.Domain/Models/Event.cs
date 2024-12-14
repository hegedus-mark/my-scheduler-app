using Scheduler.Domain.Models.Base;
using Scheduler.Domain.Shared;

namespace Scheduler.Domain.Models;

public class Event : CalendarItem
{
    private Event(Guid? id, TimeSlot timeSlot, RecurrencePattern recurrencePattern)
        : base(timeSlot, id)
    {
        RecurrencePattern = recurrencePattern;
    }

    public RecurrencePattern RecurrencePattern { get; }

    // Factory method for new events
    public static Event Create(TimeSlot timeSlot, RecurrencePattern recurrencePattern)
    {
        return new Event(null, timeSlot, recurrencePattern);
    }

    // Factory method for existing events
    public static Event Load(Guid id, TimeSlot timeSlot, RecurrencePattern recurrencePattern)
    {
        return new Event(id, timeSlot, recurrencePattern);
    }
}
