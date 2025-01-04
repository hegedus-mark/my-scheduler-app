using Scheduling.Domain.Models.Enums;

namespace Api.Models.Scheduling.Requests;

public class TaskRequest
{
    public string Name { get; set; }
    public DateTime DueDate { get; set; }
    public PriorityLevel Priority { get; set; }
    public TimeSpan Duration { get; set; }
}
