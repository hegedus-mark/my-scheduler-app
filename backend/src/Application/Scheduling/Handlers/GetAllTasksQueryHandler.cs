using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces.Repositories;
using Application.Scheduling.Queries;
using Application.Shared.Messaging;
using SharedKernel.Results;

namespace Application.Scheduling.Handlers;

public class GetAllTasksQueryHandler
    : IQueryHandler<GetAllTasksQuery, IReadOnlyCollection<TaskItemDto>>
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public GetAllTasksQueryHandler(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyCollection<TaskItemDto>>> HandleAsync(
        GetAllTasksQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _unitOfWork.TaskItems.GetAllAsync();
        var dtos = result.Select(t => t.ToDto()).ToList();
        return Result<IReadOnlyCollection<TaskItemDto>>.Success(dtos);
    }
}
