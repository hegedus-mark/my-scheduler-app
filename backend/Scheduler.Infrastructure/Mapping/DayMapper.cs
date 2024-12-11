/*using Infrastructure.Entities;
using Scheduler.Core.Enum;
using Scheduler.Core.Models;
using Scheduler.Core.Models.CalendarItems;
using Scheduler.Core.Models.Scoring;

namespace Infrastructure.Mapping;

public class DayMapper
{
    public static DayEntity ToEntity(ScheduleDay domain)
    {
        return new DayEntity
        {
            Id = domain.Id,
            Date = domain.DayDate,
            WorkStartTime = domain.WorkingHours.Start,
            WorkEndTime = domain.WorkingHours.End,
            CalendarItems = domain.CalendarItems.Select(item =>
                new CalendarItemEntity
                {
                    Id = item.Id,
                    StartTime = item.TimeSlot.Start,
                    EndTime = item.TimeSlot.End,
                    Type = item switch
                    {
                        Event => "Event",
                        ScheduledTask => "Task",
                        _ => throw new ArgumentException("Unknown calendar item type")
                    },
                    TaskName = item is ScheduledTask task ? task.Name : null,
                    Priority = item is ScheduledTask t ? (int)t.Priority : null,
                    IsRecurring = item is Event evt ? evt.IsRecurring : null
                }).ToList()
        };
    }

    public static ScheduleDay ToDomain(DayEntity entity)
    {
        var workingHours = TimeSlot.Create(entity.WorkStartTime, entity.WorkEndTime);
        var day = new ScheduleDay(entity.Date, workingHours, entity.Id);

        foreach (var itemEntity in entity.CalendarItems)
        {
            var timeSlot = TimeSlot.Create(itemEntity.StartTime, itemEntity.EndTime);

            CalendarItem item = itemEntity.Type switch
            {
                "Event" => new Event(
                    id: itemEntity.Id,
                    timeSlot: timeSlot,
                    isRecurring: itemEntity.IsRecurring ?? false
                ),
                "Task" => new ScheduledTask(
                    id: itemEntity.Id,
                    task: new TaskItem(
                        name: itemEntity.TaskName!,
                        dueDate: itemEntity.DueDate!.Value,
                        priorityLevel: (PriorityLevel)itemEntity.Priority!.Value,
                        scoringStrategy: new SimpleScoringStrategy(new UserConfig(true)),
                        duration: timeSlot.Duration
                    ),
                    timeSlot: timeSlot
                ),
                _ => throw new ArgumentException($"Unknown calendar item type: {itemEntity.Type}")
            };

            day.AddCalendarItem(item);
        }

        return day;
    }
}*/