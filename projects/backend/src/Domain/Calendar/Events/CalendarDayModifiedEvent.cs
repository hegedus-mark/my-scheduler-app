using IDomainEvent = Domain.Shared.Interfaces.IDomainEvent;

namespace Domain.Calendar.Events;

public class CalendarDayModifiedEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
