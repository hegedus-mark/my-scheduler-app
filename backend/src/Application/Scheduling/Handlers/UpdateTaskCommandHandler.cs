using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Application.Scheduling.Handlers;

public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand, TaskItemDto>
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskItemDto>> HandleAsync(
        UpdateTaskCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(command.TaskId);

        if (task is null)
            return Result<TaskItemDto>.Failure(
                Error.NotFound($"task with id: {command.TaskId} not found")
            );

        if (command.Name != null)
            task.UpdateName(command.Name);

        if (command.DueDate.HasValue)
            task.UpdateDueDate(command.DueDate.Value);

        if (command.Duration.HasValue)
            task.UpdateDuration(command.Duration.Value);

        if (command.Priority.HasValue)
            task.UpdatePriority(command.Priority.Value);

        _unitOfWork.TaskItems.Update(task);
        await _unitOfWork.SaveChangesAsync();

        return Result<TaskItemDto>.Success(task.ToDto());
    }
}
