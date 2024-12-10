using System.Text;
using Scheduler.Core.Models;

namespace Scheduler.Core.Algo;

public class UserTaskScheduler
{
    public SchedulingResult ScheduleTasks(List<Day> days, List<UnscheduledTask> unscheduledTasks)
    {
        // Validate input parameters
        if (days == null || !days.Any())
            throw new ArgumentException("Must provide at least one day for scheduling");
        if (unscheduledTasks == null || !unscheduledTasks.Any())
            throw new ArgumentException("Must provide tasks to schedule");

        var scheduledTasks = new List<ScheduledTask>();
        var failedToSchedule = new List<UnscheduledTask>();

        // Sort days chronologically, could be done outside maybe? 
        var sortedDays = days.OrderBy(d => d.DayDate).ToList();

        // Sort tasks by score in descending order and then by due date
        var sortedTasks = unscheduledTasks
            .OrderByDescending(t => t.Score)
            .ThenBy(t => t.DueDate)
            .ToList();

        foreach (var task in sortedTasks)
        {
            var taskScheduled = false;
            var requiredDuration = task.Duration;

            // Find the earliest possible day for this task
            foreach (var day in sortedDays.Where(d => d.DayDate <= task.DueDate.Date))
            {
                var suitableSlot = FindBestTimeSlot(day.FreeSlots, requiredDuration);

                if (suitableSlot != null)
                {
                    // Create a new TimeSlot for the scheduled task
                    var scheduledTimeSlot = new TimeSlot(
                        suitableSlot.Start,
                        suitableSlot.Start.Add(requiredDuration)
                    );
                    
                    // Create the ScheduledTask object
                    var scheduledTask = new ScheduledTask(task, scheduledTimeSlot);

                    // Add the task to the day's calendar
                    day.AddCalendarItem(scheduledTask);

                    // Add to our list of successfully scheduled tasks
                    scheduledTasks.Add(scheduledTask);
                    taskScheduled = true;
                    break;
                }
            }

            if (!taskScheduled)
            {
                failedToSchedule.Add(task);
            }
        }

        return new SchedulingResult(scheduledTasks, failedToSchedule);
    }

    /// <summary>
    /// Finds the best available time slot for a task with the given duration.
    /// Prioritizes earlier time slots and tries to minimize fragmentation.
    /// </summary>
    private TimeSlot? FindBestTimeSlot(List<TimeSlot> freeSlots, TimeSpan requiredDuration)
    {
        // Sort slots by start time to ensure we schedule as early as possible
        var sortedSlots = freeSlots
            .OrderBy(s => s.Start)
            .Where(s => s.Duration >= requiredDuration)
            .ToList();

        if (!sortedSlots.Any())
            return null;

        // First try to find a slot that perfectly matches the required duration
        var perfectSlot = sortedSlots
            .FirstOrDefault(s => s.Duration == requiredDuration);

        if (perfectSlot != null)
            return perfectSlot;

        // Otherwise, return the earliest slot that can accommodate the task
        return sortedSlots.First();
    }
    
}