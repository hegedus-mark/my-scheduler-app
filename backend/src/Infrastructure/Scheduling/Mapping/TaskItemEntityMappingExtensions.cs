using Domain.Scheduling.Models;
using Domain.Scheduling.Models.Enums;
using Domain.Shared.ValueObjects;
using Infrastructure.Scheduling.Entities;
using SharedKernel.Extensions;

namespace Infrastructure.Scheduling.Mapping;

public static class TaskItemMappingExtensions
{
    public static TaskItemEntity ToEntity(this TaskItem item)
    {
        var entity = new TaskItemEntity
        {
            // Core properties
            Id = item.Id,
            Name = item.Name,
            DueDate = item.DueDate,
            Duration = item.Duration,
            PriorityLevel = item.Priority,
            TaskItemStatus = item.Status,

            // Scheduling-specific properties
            StartDate = item.IsScheduled ? item.ScheduledTime?.StartDate : null,
            EndDate = item.IsScheduled ? item.ScheduledTime?.EndDate : null,

            // Failure tracking
            FailureReason = item.HasFailed ? item.FailureReason : null,
        };

        // Validation
        if (item.IsScheduled && item.ScheduledTime == null)
            throw new InvalidOperationException("Scheduled task must have scheduled time");

        return entity;
    }

    public static TaskItem ToDomain(this TaskItemEntity entity)
    {
        TaskItem task;

        task = TaskItem.Load(
            entity.Name,
            entity.DueDate,
            entity.Duration,
            entity.PriorityLevel,
            entity.Id
        );

        switch (entity.TaskItemStatus)
        {
            case TaskItemStatus.Draft:
                break;
            case TaskItemStatus.Scheduled:
                var window = CalendarTimeWindow.Create(
                    entity.StartDate!.Value.ToDateOnly(),
                    TimeSlot.Create(
                        entity.StartDate.Value.ToTimeOnly(),
                        entity.EndDate!.Value.ToTimeOnly()
                    )
                );
                task.Schedule(window);
                break;
            case TaskItemStatus.Unscheduled:
                task.MarkAsFailedToSchedule(entity.FailureReason!);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(entity.TaskItemStatus));
        }

        return task;
    }
}
