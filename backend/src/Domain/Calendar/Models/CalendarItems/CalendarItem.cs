using SharedKernel.Results;
using EntityBase = Domain.Shared.Base.EntityBase;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

namespace Domain.Calendar.Models.CalendarItems;

public class CalendarItem : EntityBase
{
    protected CalendarItem(TimeSlot timeSlot, string title, Guid? id = null)
        : base(id)
    {
        TimeSlot = timeSlot;
        Title = title;
    }

    public TimeSlot TimeSlot { get; protected set; }
    public string Title { get; }

    protected internal virtual Result<CalendarItem> UpdateTimeSlot(TimeSlot newSlot)
    {
        TimeSlot = newSlot;
        return Result<CalendarItem>.Success(this);
    }

    // Factory method for new items
    public static CalendarItem Create(TimeSlot timeSlot, string title)
    {
        return new CalendarItem(timeSlot, title);
    }

    // Factory method for existing items
    public static CalendarItem Load(Guid id, TimeSlot timeSlot, string title)
    {
        return new CalendarItem(timeSlot, title, id);
    }
}
