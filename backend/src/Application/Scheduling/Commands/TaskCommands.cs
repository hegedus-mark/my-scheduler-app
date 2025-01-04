using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.Operations.Commands;

public record UpdateTaskNameCommand(Guid TaskId, string NewName);

public record UpdateTaskDueDateCommand(Guid TaskId, DateTime NewDueDate);

public record UpdateTaskDurationCommand(Guid TaskId, TimeSpan NewDuration);

public record UpdateTaskPriorityCommand(Guid TaskId, PriorityLevel NewPriority);

public record CreateTaskCommand(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
);
