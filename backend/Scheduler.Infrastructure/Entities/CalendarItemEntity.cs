namespace Infrastructure.Entities;

public class CalendarItemEntity
{
    public Guid Id { get; set; }
    public Guid DayId { get; set; }
    public string Type { get; set; } // "Event" or "Task"
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // For tasks
    public string? TaskName { get; set; }
    public int? Priority { get; set; }
    public DateTime? DueDate { get; set; }

    // For events
    public bool? IsRecurring { get; set; }
}
