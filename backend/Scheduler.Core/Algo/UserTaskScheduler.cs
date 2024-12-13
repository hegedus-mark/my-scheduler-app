using Scheduler.Core.Extensions;
using Scheduler.Core.Models;
using Scheduler.Core.Models.CalendarItems;
using Scheduler.Core.Models.Results;
using WorkingDay = Scheduler.Core.Models.CalendarItems.WorkingDay;

namespace Scheduler.Core.Algo;

public class UserTaskScheduler
{
    public SchedulingResult ScheduleTasks(
        IReadOnlyCollection<WorkingDay> days,
        IReadOnlyCollection<TaskItem> unscheduledTasks
    )
    {
        // Validate input parameters
        if (days == null || !days.Any())
            throw new ArgumentException("Must provide at least one day for scheduling");
        if (unscheduledTasks == null || !unscheduledTasks.Any())
            throw new ArgumentException("Must provide tasks to schedule");

        var scheduledTasks = new List<ScheduledTask>();
        var failedToSchedule = new List<TaskItem>();

        //These will be sorted by outside
        var sortedDays = days.OrderBy(d => d.DayDate).ToList();

        //These should be encapsulated maybe in a new class? UnscheduledTasks ?
        var sortedTasks = unscheduledTasks
            .OrderByDescending(t => t.Score)
            .ThenBy(t => t.DueDate)
            .ToList();

        foreach (var task in sortedTasks)
        {
            var taskScheduled = false;
            var requiredDuration = task.Duration;

            // Find the earliest possible day for this task
            foreach (var day in sortedDays.Where(d => d.DayDate.IsOnOrBeforeDay(task.DueDate)))
            {
                var suitableSlot = FindBestTimeSlot(day.FreeSlots, requiredDuration);

                if (suitableSlot != null)
                {
                    var timeslot = TimeSlot.Create(
                        suitableSlot.Value.Start,
                        suitableSlot.Value.Start.Add(task.Duration)
                    );

                    var scheduledTask = day.AddScheduledTask(task, timeslot);

                    // Add to our list of successfully scheduled tasks
                    scheduledTasks.Add(scheduledTask);
                    taskScheduled = true;
                    break;
                }
            }

            if (!taskScheduled)
                failedToSchedule.Add(task);
        }

        return new SchedulingResult(scheduledTasks, failedToSchedule);
    }

    private TimeSlot? FindBestTimeSlot(
        IReadOnlyCollection<TimeSlot> freeSlots,
        TimeSpan requiredDuration
    )
    {
        // Sort slots by start time to ensure we schedule as early as possible
        var sortedSlots = freeSlots
            .OrderBy(s => s.Start)
            .Where(s => s.Duration >= requiredDuration)
            .ToList();

        if (!sortedSlots.Any())
            return null;

        TimeSlot perfectSlot;
        try
        {
            perfectSlot = sortedSlots.First(s => s.Duration == requiredDuration);
            return perfectSlot;
        }
        catch (InvalidOperationException e)
        {
            return sortedSlots.First();
        }
    }
}
