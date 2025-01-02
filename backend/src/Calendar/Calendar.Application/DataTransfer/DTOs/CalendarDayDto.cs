namespace Calendar.Application.DataTransfer.DTOs;

public class CalendarDayDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public DateTime? WorkStartTime { get; set; }
    public DateTime? WorkEndTime { get; set; }
    public List<CalendarItemDto> Reservations { get; set; } = new();
}
