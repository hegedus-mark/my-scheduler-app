namespace Domain.Calendar.Exceptions;

public class CalendarItemNotFoundException(Guid id)
    : CalendarDomainException($"CalendarItem with {id} not found ") { }
