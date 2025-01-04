using Application.Calendar.DataTransfer.DTOs.Enums;

namespace Application.Calendar.DataTransfer.DTOs;

public class CalendarItemDto
{
    public Guid Id { get; init; }
    public Guid CalendarDayId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Title { get; init; }
    public RecurrencePatternDto? RecurrencePattern { get; init; }

    public Guid? ExternalId { get; init; }
    public ExternalItemTypeDto? ExternalItemType { get; init; }
}
