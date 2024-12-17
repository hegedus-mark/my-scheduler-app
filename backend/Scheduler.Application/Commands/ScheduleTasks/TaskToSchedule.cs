using Scheduler.Shared.Enums;

namespace Scheduler.Application.Commands.ScheduleTasks;

public record TaskToSchedule(
    string Name,
    DateTime DueDate,
    PriorityLevel Priority,
    TimeSpan Duration
);
