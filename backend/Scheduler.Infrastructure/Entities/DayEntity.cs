namespace Infrastructure.Entities;

public class DayEntity
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime WorkStartTime { get; set; }
    public DateTime WorkEndTime { get; set; }
    public bool IsWorkingDay { get; set; }
    public List<CalendarItemEntity> CalendarItems { get; set; } = new();
}
