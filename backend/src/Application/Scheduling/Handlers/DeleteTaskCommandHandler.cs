using Application.Scheduling.Commands;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
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
        _unitOfWork.TaskItems.FastDeleteById(command.TaskId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
