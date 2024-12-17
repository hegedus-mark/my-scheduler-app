using Infrastructure.Entities;
using Scheduler.Domain.Calendars.Interfaces;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Base;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared;
using Scheduler.Shared.Extensions;

namespace Scheduler.Application.Mapping;

public class CalendarDayMapper : IMapper<ICalendarDay, DayEntity>
{
    private readonly IScoringStrategy _scoringStrategy;

    public CalendarDayMapper(IScoringStrategy scoringStrategy)
    {
        _scoringStrategy = scoringStrategy;
    }

    public DayEntity ToEntity(ICalendarDay day)
    {
        var entity = new DayEntity
        {
            Id = day is EntityBase entityBase ? entityBase.Id : Guid.NewGuid(),
            Date = day.DayDate.ToDateTime(TimeOnly.MinValue),
            IsWorkingDay = day.IsWorkingDay,
        };

        if (day is WorkingDay workingDay)
        {
            entity.WorkStartTime = workingDay.DayDate.ToDateTime(workingDay.WorkingHours.Start);
            entity.WorkEndTime = workingDay.DayDate.ToDateTime(workingDay.WorkingHours.End);
        }

        // Map all calendar items
        entity.CalendarItems = day
            .CalendarItems.Select(item => MapCalendarItemToEntity(item, entity.Id))
            .ToList();

        return entity;
    }

    public ICalendarDay ToDomain(DayEntity entity)
    {
        var date = DateOnly.FromDateTime(entity.Date);

        if (!entity.IsWorkingDay)
        {
            var nonWorkingDay = NonWorkingDay.Load(entity.Id, date);

            // Map events (even non-working days can have events)
            foreach (var eventEntity in entity.CalendarItems.Where(ci => ci.ItemType == "Event"))
            {
                var eventItem = MapEventToDomain(eventEntity);
                nonWorkingDay.AddEvent(eventItem);
            }

            return nonWorkingDay;
        }

        // Create working day with working hours
        var workingHours = TimeSlot.Create(
            TimeOnly.FromDateTime(entity.WorkStartTime),
            TimeOnly.FromDateTime(entity.WorkEndTime)
        );

        var workingDay = WorkingDay.Load(entity.Id, date, workingHours);

        // Map all calendar items
        foreach (var itemEntity in entity.CalendarItems)
            if (itemEntity.ItemType == "Event")
            {
                var eventItem = MapEventToDomain(itemEntity);
                workingDay.AddEvent(eventItem);
            }
            else if (itemEntity.ItemType == "ScheduledTask")
            {
                var timeSlot = TimeSlot.Create(
                    TimeOnly.FromDateTime(itemEntity.StartTime),
                    TimeOnly.FromDateTime(itemEntity.EndTime)
                );

                var taskItem = new TaskItem(
                    itemEntity.TaskName!,
                    itemEntity.DueDate!.Value,
                    itemEntity.Priority!.Value,
                    _scoringStrategy,
                    itemEntity.Duration!.Value
                );

                workingDay.AddScheduledTask(taskItem, timeSlot);
            }

        return workingDay;
    }

    private CalendarItemEntity MapCalendarItemToEntity(CalendarItem item, Guid dayId)
    {
        var entity = new CalendarItemEntity
        {
            Id = item.Id,
            DayId = dayId,
            StartTime = item.TimeSlot.Start.ToDateTime(DateOnly.FromDateTime(DateTime.Today)),
            EndTime = item.TimeSlot.End.ToDateTime(DateOnly.FromDateTime(DateTime.Today)),
        };

        if (item is Event eventItem)
        {
            entity.ItemType = "Event";
            entity.RecurrencePattern =
                eventItem.RecurrencePattern != null
                    ? new RecurrencePatternEntity
                    {
                        Id = eventItem.RecurrencePattern.Id,
                        Type = eventItem.RecurrencePattern.Type,
                        Interval = eventItem.RecurrencePattern.Interval,
                        EndDate = eventItem.RecurrencePattern.EndDate,
                        SelectedDays = eventItem.RecurrencePattern.SelectedDays,
                        CalendarItemId = entity.Id,
                    }
                    : null;
        }
        else if (item is ScheduledTask scheduledTask)
        {
            entity.ItemType = "ScheduledTask";
            entity.TaskName = scheduledTask.Name;
            entity.Priority = scheduledTask.Priority;
            entity.DueDate = scheduledTask.DueDate;
            entity.Duration = scheduledTask.OriginalTask.Duration;
        }

        return entity;
    }

    private Event MapEventToDomain(CalendarItemEntity entity)
    {
        var timeSlot = TimeSlot.Create(
            TimeOnly.FromDateTime(entity.StartTime),
            TimeOnly.FromDateTime(entity.EndTime)
        );

        var recurrencePattern =
            entity.RecurrencePattern != null
                ? new RecurrencePattern
                {
                    Type = entity.RecurrencePattern.Type,
                    Interval = entity.RecurrencePattern.Interval,
                    EndDate = entity.RecurrencePattern.EndDate,
                    SelectedDays = entity.RecurrencePattern.SelectedDays ?? 0,
                }
                : null;

        return Event.Create(timeSlot, recurrencePattern);
    }
}
