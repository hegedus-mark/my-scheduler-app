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

    protected override TaskItem MapToDomain(TaskItemEntity entity)
    {
        return entity.ToDomain();
    }

    protected override TaskItemEntity MapToEntity(TaskItem domain)
    {
        return domain.ToEntity();
    }
}
