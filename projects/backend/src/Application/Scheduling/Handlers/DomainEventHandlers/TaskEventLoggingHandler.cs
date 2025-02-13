using Application.Shared.Messaging;
using Domain.Scheduling.Events;
using Microsoft.Extensions.Logging;

namespace Application.Scheduling.Handlers.DomainEventHandlers;

public class TaskEventLoggingHandler
    : IDomainEventHandler<TaskScheduledEvent>,
        IDomainEventHandler<TaskFailedToScheduleEvent>,
        IDomainEventHandler<TaskUpdatedEvent>,
        IDomainEventHandler<TaskSchedulingRetryRequestedEvent>
{
    private readonly ILogger<TaskEventLoggingHandler> _logger;

    public TaskEventLoggingHandler(ILogger<TaskEventLoggingHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(
        TaskFailedToScheduleEvent domainEvent,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogWarning(
            "Task {TaskId} failed to schedule. Reason: {Reason}",
            domainEvent.TaskId,
            domainEvent.Reason
        );
        return Task.CompletedTask;
    }

    public Task HandleAsync(
        TaskScheduledEvent domainEvent,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation(
            "Task {TaskId} scheduled time: {ScheduledWindow}",
            domainEvent.TaskId,
            domainEvent.ScheduledTimeWindow
        );
        return Task.CompletedTask;
    }

    public Task HandleAsync(
        TaskSchedulingRetryRequestedEvent domainEvent,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation("Task {TaskId} was moved back to Draft state", domainEvent.TaskId);

        return Task.CompletedTask;
    }

    public Task HandleAsync(
        TaskUpdatedEvent domainEvent,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation(
            "Task {TaskId}'s {property} was updated to {newValue}",
            domainEvent.Id,
            domainEvent.Property,
            domainEvent.NewValue
        );

        return Task.CompletedTask;
    }
}
