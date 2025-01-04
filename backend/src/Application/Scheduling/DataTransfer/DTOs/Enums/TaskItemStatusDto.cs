using Scheduling.Domain.Enums;

namespace Application.Scheduling.DataTransfer.DTOs.Enums;

public enum TaskItemStatusDto
{
    Scheduled = TaskItemStatus.Scheduled,
    Unscheduled = TaskItemStatus.Unscheduled,
    Draft = TaskItemStatus.Draft,
}
