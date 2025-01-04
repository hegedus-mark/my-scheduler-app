using SharedKernel.Persistence;

namespace Infrastructure.Calendar.Entities;

public class CalendarDayEntity : IEntity
{
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public DateTime? WorkStartTime { get; set; }
    public DateTime? WorkEndTime { get; set; }
    public List<CalendarItemEntity> Reservations { get; set; } = new();
    public Guid Id { get; set; }
}
