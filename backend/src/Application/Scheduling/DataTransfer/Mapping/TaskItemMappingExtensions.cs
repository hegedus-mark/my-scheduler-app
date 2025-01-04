using Application.Scheduling.DataTransfer.DTOs;
using Domain.Scheduling.Models;
using Domain.Scheduling.Models.Enums;
using SharedKernel.Domain.ValueObjects;
using SharedKernel.Extensions;

namespace Application.Scheduling.DataTransfer.Mapping;

public static class TaskItemMappingExtensions
{
    public static TaskItemDto ToDto(this TaskItem item)
    {
        return new TaskItemDto
        {
            DueDate = item.DueDate,
            Duration = item.Duration,
            EndDate = item.IsScheduled ? item.ScheduledTime!.Value.EndDate : null,
            StartDate = item.IsScheduled ? item.ScheduledTime!.Value.StartDate : null,
            FailureReason = item.HasFailed ? item.FailureReason : null,
            Name = item.Name,
            PriorityLevel = item.Priority,
            TaskItemStatus = item.Status,
            Id = item.Id,
        };
    }

    public static TaskItem ToDomain(this TaskItemDto dto)
    {
        TaskItem task;

        if (dto.Id.HasValue)
            task = TaskItem.Load(
                dto.Name,
                dto.DueDate,
                dto.Duration,
                dto.PriorityLevel,
                dto.Id.Value
            );
        else
            task = TaskItem.Create(dto.Name, dto.DueDate, dto.Duration, dto.PriorityLevel);

        switch (dto.TaskItemStatus)
        {
            case TaskItemStatus.Draft:
                break;
            case TaskItemStatus.Scheduled:
                var window = CalendarTimeWindow.Create(
                    dto.StartDate!.Value.ToDateOnly(),
                    TimeSlot.Create(
                        dto.StartDate.Value.ToTimeOnly(),
                        dto.EndDate!.Value.ToTimeOnly()
                    )
                );
                task.Schedule(window);
                break;
            case TaskItemStatus.Unscheduled:
                task.MarkAsFailedToSchedule(dto.FailureReason!);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dto.TaskItemStatus));
        }

        return task;
    }
}
