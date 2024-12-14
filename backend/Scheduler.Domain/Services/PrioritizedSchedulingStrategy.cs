using Scheduler.Domain.Extensions;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Shared;
using Scheduler.Domain.Shared.Results;

namespace Scheduler.Domain.Services;

public class PrioritizedSchedulingStrategy : ISchedulingStrategy
{
    public SchedulingResult Schedule(
        IReadOnlyList<WorkingDay> availableDays,
        IReadOnlyCollection<TaskItem> tasks,
        UserScheduleConfig config
    )
    {
        if (!availableDays.Any())
            throw new ArgumentException("Must provide at least one day for scheduling");
        if (!tasks.Any())
            throw new ArgumentException("Must provide tasks to schedule");

        var scheduledTasks = new List<ScheduledTask>();
        var failedToSchedule = new List<TaskItem>();

        var sortedDays = availableDays.OrderBy(d => d.DayDate).ToList();
        var sortedTasks = tasks.OrderByDescending(t => t.Score).ThenBy(t => t.DueDate).ToList();

        foreach (var task in sortedTasks)
        {
            if (TryScheduleTask(task, sortedDays, scheduledTasks))
                continue;

            failedToSchedule.Add(task);
        }

        return new SchedulingResult(scheduledTasks, failedToSchedule);
    }

    private bool TryScheduleTask(
        TaskItem task,
        IReadOnlyList<WorkingDay> availableDays,
        List<ScheduledTask> scheduledTasks
    )
    {
        foreach (var day in availableDays.Where(d => d.DayDate.IsOnOrBeforeDay(task.DueDate)))
        {
            var suitableSlot = FindBestTimeSlot(day.FreeSlots, task.Duration);
            if (suitableSlot == null)
                continue;

            var timeSlot = TimeSlot.Create(
                suitableSlot.Value.Start,
                suitableSlot.Value.Start.Add(task.Duration)
            );

            var scheduledTask = day.AddScheduledTask(task, timeSlot);
            scheduledTasks.Add(scheduledTask);
            return true;
        }

        return false;
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

        //Find a slot where the duration matches the requiredDuration exactly!
        var perfectSlot = sortedSlots.Where(s => s.Duration == requiredDuration).ToList();
        if (perfectSlot.Any())
            return perfectSlot.First();
        return sortedSlots.First();
    }
}
