namespace Scheduler.Domain.Shared;

public abstract class CalendarItem : EntityBase
{
    //For new CalendarItems
    protected CalendarItem(TimeSlot timeSlot)
        : base(Guid.NewGuid())
    {
        TimeSlot = timeSlot;
    }

    //For existing CalendarItems
    protected CalendarItem(Guid id, TimeSlot timeSlot)
        : base(id)
    {
        TimeSlot = timeSlot;
    }

    public TimeSlot TimeSlot { get; private set; }
}
