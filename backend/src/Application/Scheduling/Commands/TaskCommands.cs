using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;
using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.Commands;

public record UpdateTaskNameCommand(Guid TaskId, string NewName) : ICommand<TaskItemDto>;

public record UpdateTaskDueDateCommand(Guid TaskId, DateTime NewDueDate) : ICommand<TaskItemDto>;

public record UpdateTaskDurationCommand(Guid TaskId, TimeSpan NewDuration) : ICommand<TaskItemDto>;

public record UpdateTaskPriorityCommand(Guid TaskId, PriorityLevel NewPriority)
    : ICommand<TaskItemDto>;

public record CreateTaskCommand(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
) : ICommand<TaskItemDto>;
