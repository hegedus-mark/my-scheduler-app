namespace Api.Models.Requests;

public class ScheduleTasksRequest
{
    public List<TaskRequestDto> Tasks { get; set; }
    public DateTime? WindowStart { get; set; }
    public DateTime? WindowEnd { get; set; }
}
