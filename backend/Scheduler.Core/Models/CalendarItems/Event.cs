namespace Scheduler.Core.Models.CalendarItems;

public class Event : CalendarItem
{
    public RecurrencePattern RecurrencePattern { get; }


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
}