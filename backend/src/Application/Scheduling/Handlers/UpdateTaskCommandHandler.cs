using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
using Domain.Scheduling.Models;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Application.Scheduling.Handlers;

public abstract class UpdateTaskCommandHandlerBase<TCommand>
    : ICommandHandler<TCommand, TaskItemDto>
    where TCommand : IUpdateTaskCommand
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    protected UpdateTaskCommandHandlerBase(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskItemDto>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(command.TaskId);

        if (task is null)
            return Result<TaskItemDto>.Failure(
                Error.NotFound($"task with id: ${command.TaskId} not found")
            );

        await UpdateTaskAsync(task, command);

        await _unitOfWork.TaskItems.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return Result<TaskItemDto>.Success(task.ToDto());
    }

    protected abstract Task UpdateTaskAsync(TaskItem task, TCommand command);
}

public class UpdateTaskNameCommandHandler : UpdateTaskCommandHandlerBase<UpdateTaskNameCommand>
{
    public UpdateTaskNameCommandHandler(ISchedulingUnitOfWork unitOfWork)
        : base(unitOfWork) { }

    protected override Task UpdateTaskAsync(TaskItem task, UpdateTaskNameCommand command)
    {
        task.UpdateName(command.NewName);
        return Task.CompletedTask;
    }
}

public class UpdateTaskDueDateCommandHandler
    : UpdateTaskCommandHandlerBase<UpdateTaskDueDateCommand>
{
    public UpdateTaskDueDateCommandHandler(ISchedulingUnitOfWork unitOfWork)
        : base(unitOfWork) { }

    protected override Task UpdateTaskAsync(TaskItem task, UpdateTaskDueDateCommand command)
    {
        task.UpdateDueDate(command.NewDueDate);
        return Task.CompletedTask;
    }
}

public class UpdateTaskDurationCommandHandler
    : UpdateTaskCommandHandlerBase<UpdateTaskDurationCommand>
{
    public UpdateTaskDurationCommandHandler(ISchedulingUnitOfWork unitOfWork)
        : base(unitOfWork) { }

    protected override Task UpdateTaskAsync(TaskItem task, UpdateTaskDurationCommand command)
    {
        task.UpdateDuration(command.NewDuration);
        return Task.CompletedTask;
    }
}

public class UpdateTaskPriorityCommandCommandHandler
    : UpdateTaskCommandHandlerBase<UpdateTaskPriorityCommand>
{
    public UpdateTaskPriorityCommandCommandHandler(ISchedulingUnitOfWork unitOfWork)
        : base(unitOfWork) { }

    protected override Task UpdateTaskAsync(TaskItem task, UpdateTaskPriorityCommand command)
    {
        task.UpdatePriority(command.NewPriority);
        return Task.CompletedTask;
    }
}
