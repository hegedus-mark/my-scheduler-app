namespace Application.Scheduling.CalendarIntegration.DTOs;

public class ReserveCalendarSlotRequest
{
    public Guid TaskId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Title { get; set; }
}
