using Scheduler.Domain.Shared.Enums;

namespace Scheduler.Application.Entities;

public class UserScheduleConfigEntity
{
    public Guid Id { get; set; }
    public TimeSpan DefaultWorkStartTime { get; set; }
    public TimeSpan DefaultWorkEndTime { get; set; }
    public DaysOfWeek WorkingDays { get; set; }
    public TimeSpan MinimumTaskDuration { get; set; }
}
