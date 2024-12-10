using System.Text;

namespace Scheduler.Core.Models;

/// <summary>
/// Schedules tasks across available days, optimizing for task scores while respecting due dates.
/// Returns a collection of ScheduledTask objects representing the final schedule.
/// </summary>
public class SchedulingResult
{
    public List<ScheduledTask> ScheduledTasks { get; }
    public List<UnscheduledTask> UnscheduledTasks { get; }
        
    public SchedulingResult(List<ScheduledTask> scheduledTasks, List<UnscheduledTask> unscheduledTasks)
    {
        ScheduledTasks = scheduledTasks;
        UnscheduledTasks = unscheduledTasks;
    }
    
    public string GetScheduleSummary(SchedulingResult result)
    {
        var summary = new StringBuilder();
        foreach (var task in result.ScheduledTasks.OrderBy(t => t.TimeSlot.Start))
        {
            summary.AppendLine($"{task.Name}: {task.TimeSlot.Start:t} - {task.TimeSlot.End:t} " +
                               $"(Score: {task.OriginalTask.Score}, Due: {task.OriginalTask.DueDate:d})");
        }
    
        if (result.UnscheduledTasks.Any())
        {
            summary.AppendLine("\nFailed to schedule:");
            foreach (var task in result.UnscheduledTasks)
            {
                summary.AppendLine($"{task.Name} (Due: {task.DueDate:d}, Score: {task.Score})");
            }
        }
        return summary.ToString();
    }
}