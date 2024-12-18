using Scheduler.Domain.Shared.Enums;

namespace Scheduler.Application.Entities;

public class RecurrencePatternEntity
{
    public Guid Id { get; set; }
    public Guid CalendarItemId { get; set; }
    public RecurrenceType Type { get; set; }
    public int Interval { get; set; }
    public DateTime? EndDate { get; set; }
    public DaysOfWeek? SelectedDays { get; set; }

    // Navigation property
    public CalendarItemEntity CalendarItem { get; set; }
}
