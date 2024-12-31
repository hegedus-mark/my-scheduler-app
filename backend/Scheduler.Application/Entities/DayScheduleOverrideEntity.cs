namespace Scheduler.Application.Entities;

public class DayScheduleOverrideEntity
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public TimeSpan? CustomWorkStartTime { get; set; }
    public TimeSpan? CustomWorkEndTime { get; set; }
    public string? OverrideReason { get; set; }
}
