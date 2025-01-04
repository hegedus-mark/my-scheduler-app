using Domain.Scheduling.Models;
using Domain.Scheduling.Results;
using CalendarTimeWindow = Domain.Shared.ValueObjects.CalendarTimeWindow;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

namespace Domain.Scheduling.Services;

public class PrioritizedSchedulingStrategy : ISchedulingStrategy
{
    private readonly IScoringStrategy _scoringStrategy;

    public PrioritizedSchedulingStrategy(IScoringStrategy scoringStrategy)
    {
        _scoringStrategy = scoringStrategy;
    }

    public SchedulingResult Schedule(
        IReadOnlyCollection<TaskItem> tasks,
        IReadOnlyList<CalendarTimeWindow> availableWindows
    )
    {
        if (!availableWindows.Any())
            throw new ArgumentException("Cannot schedule tasks without available time windows");
        if (!tasks.Any())
            throw new ArgumentException("No tasks provided for scheduling");

        // Sort tasks by priority score and due date
        var sortedTasks = tasks
            .OrderByDescending(t => t.CalculateScore(_scoringStrategy))
            .ThenBy(t => t.DueDate)
            .ToList();

        // Group available windows by date for efficient processing
        var windowsByDate = availableWindows
            .GroupBy(w => w.Date)
            .ToDictionary(g => g.Key, g => g.OrderBy(w => w.TimeSlot.Start).ToList());

        var scheduledTasks = new List<TaskItem>();
        var failedToSchedule = new List<TaskItem>();

        foreach (var task in sortedTasks)
            if (TryScheduleTask(task, windowsByDate))
            {
                scheduledTasks.Add(task);

                // After successfully scheduling, we need to update available windows
                RemoveOrSplitUsedWindow(windowsByDate, task);
            }
            else
            {
                // If we couldn't schedule the task, mark it as failed
                task.MarkAsFailedToSchedule("No suitable time window found within task's deadline");
                failedToSchedule.Add(task);
            }

        return SchedulingResult.Create(scheduledTasks, failedToSchedule);
    }

    private bool TryScheduleTask(
        TaskItem task,
        Dictionary<DateOnly, List<CalendarTimeWindow>> windowsByDate
    )
    {
        var dueDate = DateOnly.FromDateTime(task.DueDate);

        // Consider only dates up to the due date
        var availableDates = windowsByDate
            .Keys.Where(date => date <= dueDate)
            .OrderBy(date => date);

        foreach (var date in availableDates)
        {
            var availableWindows = windowsByDate[date];
            var bestWindow = FindBestTimeWindow(availableWindows, task.Duration);

            if (bestWindow == null)
                continue;

            // Create a new window that exactly matches the task duration
            var taskWindow = CalendarTimeWindow.Create(
                bestWindow.Value.Date,
                TimeSlot.Create(
                    bestWindow.Value.TimeSlot.Start,
                    bestWindow.Value.TimeSlot.Start.Add(task.Duration)
                )
            );

            // Try to schedule the task in this window
            task.Schedule(taskWindow);

            return true;
        }

        return false;
    }

    private CalendarTimeWindow? FindBestTimeWindow(
        List<CalendarTimeWindow> availableWindows,
        TimeSpan requiredDuration
    )
    {
        // First, filter windows that are large enough
        var suitableWindows = availableWindows
            .Where(w => w.CanAccommodate(requiredDuration))
            .ToList();

        if (!suitableWindows.Any())
            return null;

        // Try to find a perfect fit first
        var perfectFit = suitableWindows.FirstOrDefault(w =>
            w.TimeSlot.Duration == requiredDuration
        );

        if (perfectFit != null)
            return perfectFit;

        // If no perfect fit, get the earliest window that's large enough
        return suitableWindows.OrderBy(w => w.TimeSlot.Start).First();
    }

    private void RemoveOrSplitUsedWindow(
        Dictionary<DateOnly, List<CalendarTimeWindow>> windowsByDate,
        TaskItem scheduledTask
    )
    {
        var usedWindow = scheduledTask.ScheduledTime!;
        var dateWindows = windowsByDate[usedWindow.Value.Date];

        // Find the original window that was used
        var originalWindow = dateWindows.First(w => w.CoversTime(usedWindow.Value));
        dateWindows.Remove(originalWindow);

        // If there's remaining time before the task
        if (originalWindow.TimeSlot.Start < usedWindow.Value.TimeSlot.Start)
        {
            var beforeWindow = CalendarTimeWindow.Create(
                originalWindow.Date,
                TimeSlot.Create(originalWindow.TimeSlot.Start, usedWindow.Value.TimeSlot.Start)
            );
            dateWindows.Add(beforeWindow);
        }

        // If there's remaining time after the task
        if (originalWindow.TimeSlot.End > usedWindow.Value.TimeSlot.End)
        {
            var afterWindow = CalendarTimeWindow.Create(
                originalWindow.Date,
                TimeSlot.Create(usedWindow.Value.TimeSlot.End, originalWindow.TimeSlot.End)
            );
            dateWindows.Add(afterWindow);
        }

        // Re-sort windows if any were added
        if (dateWindows.Count > 0)
            windowsByDate[usedWindow.Value.Date] = dateWindows
                .OrderBy(w => w.TimeSlot.Start)
                .ToList();
    }
}
