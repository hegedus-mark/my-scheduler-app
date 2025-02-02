namespace Api.Models.Scheduling.Requests;

public class ScheduleTasksRequest
{
    public List<TaskRequest> Tasks { get; set; }
    public DateTime? WindowStart { get; set; }
    public DateTime? WindowEnd { get; set; }
}
