using Infrastructure.Entities;
using Scheduler.Domain.Models.Configuration;

namespace Scheduler.Application.Mapping;

public class UserScheduleConfigMapper : IMapper<UserScheduleConfig, UserScheduleConfigEntity>
{
    public UserScheduleConfigEntity ToEntity(UserScheduleConfig domain)
    {
        return new UserScheduleConfigEntity
        {
            DefaultWorkStartTime = domain.DefaultWorkStartTime.ToTimeSpan(),
            DefaultWorkEndTime = domain.DefaultWorkEndTime.ToTimeSpan(),
            WorkingDays = domain.WorkingDays,
            MinimumTaskDuration = domain.MinimumTaskDuration,
        };
    }

    public UserScheduleConfig ToDomain(UserScheduleConfigEntity entity)
    {
        return new UserScheduleConfig(
            TimeOnly.FromTimeSpan(entity.DefaultWorkStartTime),
            TimeOnly.FromTimeSpan(entity.DefaultWorkEndTime),
            entity.WorkingDays,
            entity.MinimumTaskDuration
        );
    }
}
