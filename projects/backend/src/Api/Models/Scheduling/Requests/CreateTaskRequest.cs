using Domain.Scheduling.Models.Enums;

namespace Api.Models.Scheduling.Requests;

public record CreateTaskRequest(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
);
