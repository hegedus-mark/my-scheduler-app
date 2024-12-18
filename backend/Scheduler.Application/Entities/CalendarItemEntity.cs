using Scheduler.Shared.Enums;

namespace Scheduler.Application.Entities;

public class CalendarItemEntity
{
    public Guid Id { get; set; }
    public Guid DayId { get; set; }
    public string ItemType { get; set; } // "Event" or "ScheduledTask"
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // Navigation property
    public DayEntity Day { get; set; }

    // For scheduled tasks
    public string? TaskName { get; set; }
    public PriorityLevel? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeSpan? Duration { get; set; }

    // For events
    public RecurrencePatternEntity? RecurrencePattern { get; set; }
}
