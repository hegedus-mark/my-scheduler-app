using Scheduler.Domain.Shared;

namespace Scheduler.Domain.Calendars.Events;

public class Event : CalendarItem
{
    // Constructor for new events
    public Event(TimeSlot timeSlot, RecurrencePattern recurrencePattern)
        : base(timeSlot)
    {
        RecurrencePattern = recurrencePattern;
    }

    // Constructor for existing events
    public Event(Guid id, TimeSlot timeSlot, RecurrencePattern recurrencePattern)
        : base(id, timeSlot)
    {
        RecurrencePattern = recurrencePattern;
    }

    public RecurrencePattern RecurrencePattern { get; }
}
