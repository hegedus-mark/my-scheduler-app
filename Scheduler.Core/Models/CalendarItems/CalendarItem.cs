namespace Scheduler.Core.Models.CalendarItems;

public abstract class CalendarItem
{
    public Guid Id { get; private set; }
    public TimeSlot TimeSlot { get; private set; }
    
    //For new CalendarItems
    protected CalendarItem(TimeSlot timeSlot)
        : this(Guid.NewGuid(), timeSlot)
    {
    }

    //For existing CalendarItems
    protected CalendarItem(Guid id, TimeSlot timeSlot)
    {
        Id = id;
        TimeSlot = timeSlot;
    }
}