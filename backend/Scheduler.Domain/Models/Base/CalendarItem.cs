using Scheduler.Shared.ValueObjects;

namespace Scheduler.Domain.Models.Base;

public abstract class CalendarItem : EntityBase
{
    protected CalendarItem(TimeSlot timeSlot, Guid? id = null)
        : base(id)
    {
        TimeSlot = timeSlot;
    }

    public TimeSlot TimeSlot { get; private set; }
}
