namespace Domain.Calendar.Exceptions;

public class TimeConflictException()
    : CalendarDomainException("New time slot conflicts with existing items") { }
