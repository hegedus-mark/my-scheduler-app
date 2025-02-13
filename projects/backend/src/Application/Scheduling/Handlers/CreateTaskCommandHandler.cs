using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces.Repositories;
using Application.Shared.Messaging;
using Application.Shared.Results;
using Domain.Scheduling.Models;

namespace Application.Scheduling.Handlers;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, TaskItemDto>
{
    private readonly IMediator _mediator;
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(ISchedulingUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
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

        var domainEvents = task.DomainEvents;
        foreach (var domainEvent in domainEvents)
            await _mediator.PublishAsync(domainEvent);
        task.ClearDomainEvents();

        return Result<TaskItemDto>.Success(task.ToDto());
    }
}
