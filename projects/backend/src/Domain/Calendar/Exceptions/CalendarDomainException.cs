using Domain.Shared.Exceptions;

namespace Domain.Calendar.Exceptions;

public class CalendarDomainException(string? message) : DomainException(message) { }
