using Application.Scheduling.DataTransfer.DTOs.Enums;

namespace Api.Models.Scheduling.Requests;

public class TaskRequest
{
    public string Name { get; set; }
    public DateTime DueDate { get; set; }
    public PriorityLevelDto Priority { get; set; }
    public TimeSpan Duration { get; set; }
}
