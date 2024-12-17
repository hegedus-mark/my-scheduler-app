using Scheduler.Domain.ValueObjects;

namespace Scheduler.Application.Commands.ScheduleTasks;

public class ScheduleTasksCommand
{
    public ScheduleTasksCommand(
        IReadOnlyCollection<TaskToSchedule> tasks,
        DateRange? schedulingWindow = null
    )
    {
        Tasks = tasks;
        SchedulingWindow = schedulingWindow;
    }

    public IReadOnlyCollection<TaskToSchedule> Tasks { get; }
    public DateRange? SchedulingWindow { get; }
}
