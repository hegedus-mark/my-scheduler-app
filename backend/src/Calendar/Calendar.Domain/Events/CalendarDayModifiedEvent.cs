using SharedKernel.Domain.Interfaces;

namespace Calendar.Domain.Events;

public class CalendarDayModifiedEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
