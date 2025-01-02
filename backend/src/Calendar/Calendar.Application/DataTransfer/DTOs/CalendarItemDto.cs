namespace Calendar.Application.DataTransfer.DTOs;

public class CalendarItemDto
{
    public Guid Id { get; set; }
    public Guid CalendarDayId { get; set; }
    public string Type { get; set; } // "Event" or "TaskReservation"
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string Title { get; set; }

    // For Events
    public RecurrencePatternDto? RecurrencePattern { get; set; }

    // For TaskReservations
    public Guid? ExternalTaskId { get; set; }
}
