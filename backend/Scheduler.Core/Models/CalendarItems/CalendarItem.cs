namespace Scheduler.Core.Models.CalendarItems;

public abstract class CalendarItem : EntityBase
{
    public TimeSlot TimeSlot { get; private set; }

    //For new CalendarItems
    protected CalendarItem(TimeSlot timeSlot)
        : base(Guid.NewGuid())
    {
        TimeSlot = timeSlot;
    }

    //For existing CalendarItems
    protected CalendarItem(Guid id, TimeSlot timeSlot) : base(id)
    {
        TimeSlot = timeSlot;
    }
}