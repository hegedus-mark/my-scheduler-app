using Calendar.Domain.Models.Enums;
using SharedKernel.Persistence;

namespace Infrastructure.Calendar.Entities;

public class CalendarItemEntity : IEntity
{
    public Guid Id { get; init; }
    public Guid CalendarDayId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Title { get; init; }

    public string? RecurrenceType { get; init; }
    public int? RecurrenceInterval { get; init; }
    public DateTime? RecurrenceEndDate { get; init; }
    public DaysOfWeek? RecurrenceSelectedDays { get; init; }

    public Guid? ExternalId { get; init; }
    public ExternalItemType? ExternalItemType { get; init; }
}
