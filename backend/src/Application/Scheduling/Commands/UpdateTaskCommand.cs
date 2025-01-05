using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;
using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.Commands;

public record UpdateTaskCommand(
    Guid TaskId,
    string? Name = null,
    DateTime? DueDate = null,
    TimeSpan? Duration = null,
    PriorityLevel? Priority = null
) : ICommand<TaskItemDto>;
