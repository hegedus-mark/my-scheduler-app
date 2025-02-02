using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;
using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.Commands;

public record CreateTaskCommand(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
) : ICommand<TaskItemDto>;
