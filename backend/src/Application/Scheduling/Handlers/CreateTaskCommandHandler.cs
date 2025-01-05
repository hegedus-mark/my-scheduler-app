using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces.Repositories;
using Domain.Scheduling.Models;
using SharedKernel.Results;

namespace Application.Scheduling.Handlers;

public class CreateTaskCommandHandler
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskItemDto>> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var task = TaskItem.Create(
            command.Name,
            command.DueDate,
            command.Duration,
            command.Priority
        );

        await _unitOfWork.TaskItems.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return Result<TaskItemDto>.Success(task.ToDto());
    }
}
