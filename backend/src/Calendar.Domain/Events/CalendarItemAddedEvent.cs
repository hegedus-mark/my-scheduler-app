using SharedKernel.Domain.Interfaces;

namespace Calendar.Domain.Events;

public class CalendarItemAddedEvent : IDomainEvent
{
    public CalendarItemAddedEvent(Guid id, Guid itemId)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
