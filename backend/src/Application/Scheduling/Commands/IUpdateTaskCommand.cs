using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;

namespace Application.Scheduling.Commands;

public interface IUpdateTaskCommand : ICommand<TaskItemDto>
{
    Guid TaskId { get; }
}
