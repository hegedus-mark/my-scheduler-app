using SharedKernel.Domain.Interfaces;

namespace Domain.Calendar.Events;

public class CalendarDayModifiedEvent : IDomainEvent
{
    public DateTime OccurredOn => DateTime.Now;
}
