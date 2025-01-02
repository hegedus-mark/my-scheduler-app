using System.Text;
using Scheduling.Domain.Models;

namespace Scheduling.Domain.Results;

public class SchedulingResult
{
    // Private constructor ensures the factory method is used and all validation happens in one place
    private SchedulingResult(
        IReadOnlyList<TaskItem> scheduledTasks,
        IReadOnlyList<TaskItem> failedTasks
    )
    {
        // We want immutable collections to prevent modifications after creation
        ScheduledTasks = scheduledTasks;
        FailedTasks = failedTasks;
    }

    public IReadOnlyList<TaskItem> ScheduledTasks { get; }
    public IReadOnlyList<TaskItem> FailedTasks { get; }

    // These properties help quickly assess the outcome
    public bool HasFailedTasks => FailedTasks.Any();
    public bool IsCompletelyScheduled => !HasFailedTasks;
    public int TotalTasksProcessed => ScheduledTasks.Count + FailedTasks.Count;

    // Factory method with validation
    public static SchedulingResult Create(
        IEnumerable<TaskItem> scheduledTasks,
        IEnumerable<TaskItem> failedTasks
    )
    {
        var scheduled = scheduledTasks?.ToList() ?? new List<TaskItem>();
        var failed = failedTasks?.ToList() ?? new List<TaskItem>();

        // Validate that tasks are in the correct state
        if (scheduled.Any(t => !t.IsScheduled))
            throw new ArgumentException("All tasks in scheduledTasks must be in Scheduled state");

        if (failed.Any(t => !t.HasFailed))
            throw new ArgumentException("All tasks in failedTasks must be in Failed state");

        return new SchedulingResult(scheduled, failed);
    }

    public string GetScheduleSummary()
    {
        var summary = new StringBuilder();

        summary.AppendLine("Scheduling Results Summary:");
        summary.AppendLine($"Total tasks processed: {TotalTasksProcessed}");
        summary.AppendLine($"Successfully scheduled: {ScheduledTasks.Count}");
        summary.AppendLine($"Failed to schedule: {FailedTasks.Count}");

        if (ScheduledTasks.Any())
        {
            summary.AppendLine("\nScheduled Tasks:");
            foreach (
                var task in ScheduledTasks
                    .OrderBy(t => t.ScheduledTime!.Value.Date)
                    .ThenBy(t => t.ScheduledTime!.Value.TimeSlot.Start)
            )
                summary.AppendLine(
                    $"- {task.Name}: {task.ScheduledTime!.Value.Date:d} at "
                        + $"{task.ScheduledTime.Value.TimeSlot.Start:t} - {task.ScheduledTime.Value.TimeSlot.End:t}"
                );
        }

        if (FailedTasks.Any())
        {
            summary.AppendLine("\nFailed Tasks:");
            foreach (var task in FailedTasks.OrderBy(t => t.DueDate))
                // Use reflection or a property to get the failure reason
                summary.AppendLine($"- {task.Name} (Due: {task.DueDate:d})");
        }

        return summary.ToString();
    }
}
