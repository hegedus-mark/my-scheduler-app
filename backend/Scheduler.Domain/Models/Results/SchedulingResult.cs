using System.Text;
using Scheduler.Domain.Models;

namespace Scheduler.Domain.Shared.Results;

public class SchedulingResult
{
    public SchedulingResult(List<ScheduledTask> scheduledTasks, List<TaskItem> unscheduledTasks)
    {
        ScheduledTasks = scheduledTasks;
        UnscheduledTasks = unscheduledTasks;
    }

    public List<ScheduledTask> ScheduledTasks { get; }
    public List<TaskItem> UnscheduledTasks { get; }

    public string GetScheduleSummary()
    {
        var summary = new StringBuilder();
        foreach (var task in ScheduledTasks.OrderBy(t => t.TimeSlot.Start))
            summary.AppendLine(
                $"{task.Name}: {task.TimeSlot.Start:t} - {task.TimeSlot.End:t} "
                    + $"(Score: {task.OriginalTask.Score}, Due: {task.OriginalTask.DueDate:d})"
            );

        if (UnscheduledTasks.Any())
        {
            summary.AppendLine("\nTasks that couldn't be scheduled:");
            foreach (var task in UnscheduledTasks)
                summary.AppendLine($"{task.Name} (Due: {task.DueDate:d}, Score: {task.Score})");
        }

        return summary.ToString();
    }
}