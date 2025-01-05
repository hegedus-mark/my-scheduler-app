using Application.Scheduling.Commands;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Application.Scheduling.Handlers;

public class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand>
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        DeleteTaskCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(command.TaskId);

        if (task is null)
            return Result.Failure(Error.NotFound($"task with id: {command.TaskId} not found"));

        await _unitOfWork.TaskItems.RemoveAsync(task);

        return Result.Success();
    }
}
