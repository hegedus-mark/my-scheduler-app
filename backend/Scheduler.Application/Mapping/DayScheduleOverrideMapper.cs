using Scheduler.Application.Entities;
using Scheduler.Application.Interfaces.Mapping;
using Scheduler.Domain.Models.Configuration;

namespace Scheduler.Application.Mapping;

public class DayScheduleOverrideMapper : IMapper<DayScheduleOverride, DayScheduleOverrideEntity>
{
    public DayScheduleOverrideEntity ToEntity(DayScheduleOverride domain)
    {
        return new DayScheduleOverrideEntity
        {
            Date = domain.Date.ToDateTime(TimeOnly.MinValue),
            IsWorkingDay = domain.IsWorkingDay,
            CustomWorkStartTime = domain.CustomWorkingHours?.Start.ToTimeSpan(),
            CustomWorkEndTime = domain.CustomWorkingHours?.End.ToTimeSpan(),
            OverrideReason = domain.OverrideReason,
        };
    }

    public DayScheduleOverride ToDomain(DayScheduleOverrideEntity entity)
    {
        if (entity.CustomWorkStartTime.HasValue && entity.CustomWorkEndTime.HasValue)
            return DayScheduleOverride.CreateWorkingHoursOverride(
                DateOnly.FromDateTime(entity.Date),
                TimeOnly.FromTimeSpan(entity.CustomWorkStartTime.Value),
                TimeOnly.FromTimeSpan(entity.CustomWorkEndTime.Value),
                entity.OverrideReason
            );

        return DayScheduleOverride.CreateWorkingDayOverride(
            DateOnly.FromDateTime(entity.Date),
            entity.IsWorkingDay,
            entity.OverrideReason
        );
    }
}
