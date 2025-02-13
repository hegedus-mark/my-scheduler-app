using Application.Scheduling.Commands;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
using Application.Shared.Results;

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
        //TODO: Maybe this FastDelete isn't needed at all
        //It would be better to use a regular delete and raise a domain event
        _unitOfWork.TaskItems.FastDeleteById(command.TaskId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
