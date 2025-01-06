using Application.Scheduling.Interfaces.Repositories;
using Domain.Scheduling.Models;
using Infrastructure.Scheduling.Entities;
using Infrastructure.Scheduling.Mapping;
using Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Scheduling.Repositories;

internal class TaskItemRepository : BaseRepository<TaskItem, TaskItemEntity>, ITaskItemRepository
{
    public TaskItemRepository(DbContext context)
        : base(context) { }

    public void FastDeleteById(Guid id)
    {
        DbSet.Where(e => e.Id == id).ExecuteDelete();
    }

    protected override TaskItem MapToDomain(TaskItemEntity entity)
    {
        return entity.ToDomain();
    }

    protected override TaskItemEntity MapToEntity(TaskItem domain)
    {
        return domain.ToEntity();
    }

    protected override void MapToExistingEntity(TaskItem domain, TaskItemEntity entity)
    {
        entity.Name = domain.Name;
        entity.DueDate = domain.DueDate;
        entity.Duration = domain.Duration;
        entity.PriorityLevel = domain.Priority;
        entity.TaskItemStatus = domain.Status;
        entity.StartDate = domain.IsScheduled ? domain.ScheduledTime?.StartDate : null;
        entity.EndDate = domain.IsScheduled ? domain.ScheduledTime?.EndDate : null;
        entity.FailureReason = domain.HasFailed ? domain.FailureReason : null;
    }
}
