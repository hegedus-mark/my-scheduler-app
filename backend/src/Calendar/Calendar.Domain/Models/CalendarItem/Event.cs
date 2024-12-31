using Calendar.Domain.ValueObjects;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Models.CalendarItem;

public class Event : CalendarItem
{
    private Event(Guid? id, TimeSlot timeSlot, string title, RecurrencePattern recurrencePattern)
        : base(timeSlot, title, id)
    {
        RecurrencePattern = recurrencePattern;
    }

    public RecurrencePattern RecurrencePattern { get; }

    // Factory method for new events
    public static Event Create(TimeSlot timeSlot, string title, RecurrencePattern recurrencePattern)
    {
        return new Event(null, timeSlot, title, recurrencePattern);
    }

    // Factory method for existing events
    public static Event Load(
        Guid id,
        TimeSlot timeSlot,
        string title,
        RecurrencePattern recurrencePattern
    )
    {
        return new Event(id, timeSlot, title, recurrencePattern);
    }
}
