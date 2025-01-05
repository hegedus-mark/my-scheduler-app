using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;
using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.Commands;

public record UpdateTaskNameCommand(Guid TaskId, string NewName) : IUpdateTaskCommand;

public record UpdateTaskDueDateCommand(Guid TaskId, DateTime NewDueDate) : IUpdateTaskCommand;

public record UpdateTaskDurationCommand(Guid TaskId, TimeSpan NewDuration) : IUpdateTaskCommand;

public record UpdateTaskPriorityCommand(Guid TaskId, PriorityLevel NewPriority)
    : IUpdateTaskCommand;

public record CreateTaskCommand(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
) : ICommand<TaskItemDto>;
