using SharedKernel.Common.Results;
using SharedKernel.Domain.Base;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Models.CalendarItems;

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
}
