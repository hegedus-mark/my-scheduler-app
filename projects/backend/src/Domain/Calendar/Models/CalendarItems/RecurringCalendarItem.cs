using Domain.Calendar.ValueObjects;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

namespace Domain.Calendar.Models.CalendarItems;

public class RecurringCalendarItem : CalendarItem
{
    private RecurringCalendarItem(
        Guid? id,
        TimeSlot timeSlot,
        string title,
        RecurrencePattern recurrencePattern
    )
        : base(timeSlot, title, id)
    {
        RecurrencePattern = recurrencePattern;
    }

    public RecurrencePattern RecurrencePattern { get; }

    // Factory method for new items
    public static RecurringCalendarItem Create(
        TimeSlot timeSlot,
        string title,
        RecurrencePattern recurrencePattern
    )
    {
        return new RecurringCalendarItem(null, timeSlot, title, recurrencePattern);
    }

    // Factory method for existing items
    public static RecurringCalendarItem Load(
        Guid id,
        TimeSlot timeSlot,
        string title,
        RecurrencePattern recurrencePattern
    )
    {
        return new RecurringCalendarItem(id, timeSlot, title, recurrencePattern);
    }
}
