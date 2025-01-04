using SharedKernel.Domain.Interfaces;

namespace Domain.Calendar.Events;

public class CalendarItemAddedEvent : IDomainEvent
{
    public CalendarItemAddedEvent(Guid id, Guid itemId)
    {
        throw new NotImplementedException();
    }

    public DateTime OccurredOn => DateTime.Now;
}
