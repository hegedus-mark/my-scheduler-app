using Domain.Calendar.Models.Enums;

namespace Application.Calendar.DataTransfer.DTOs;

public class CalendarItemDto
{
    public Guid Id { get; init; }
    public Guid CalendarDayId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Title { get; init; }

    //Recurrence Pattern
    public RecurrenceType? RecurrenceType { get; init; }
    public int? RecurrenceInterval { get; init; }
    public DateTime? RecurrenceEndDate { get; init; }
    public DaysOfWeek? RecurrenceSelectedDays { get; init; }

    //External Reference
    public Guid? ExternalId { get; init; }
    public ExternalItemType? ExternalItemType { get; init; }
}
