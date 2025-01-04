using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

namespace Domain.Calendar.Events;

public class CalendarItemRescheduledEvent : IDomainEvent
{
    public CalendarItemRescheduledEvent(Guid id, Guid itemId, TimeSlot newSlot)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
