using SharedKernel.Domain.Interfaces;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Events;

public class CalendarItemRescheduledEvent : IDomainEvent
{
    public CalendarItemRescheduledEvent(Guid id, Guid itemId, TimeSlot newSlot)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
