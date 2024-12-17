using Scheduler.Shared.Enums;

namespace SchedularPrototype.Models.Requests;

public class TaskRequestDto
{
    public string Name { get; set; }
    public DateTime DueDate { get; set; }
    public PriorityLevel Priority { get; set; }
    public TimeSpan Duration { get; set; }
}
