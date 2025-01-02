using Scheduling.Domain.Enums;

namespace Scheduling.Application.DataTransfer.DTOs.Enums;

public enum TaskItemStatusDto
{
    Scheduled = TaskItemStatus.Scheduled,
    Unscheduled = TaskItemStatus.Unscheduled,
    Draft = TaskItemStatus.Draft,
}
