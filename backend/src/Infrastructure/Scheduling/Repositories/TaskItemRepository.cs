using Application.Scheduling.Contracts.Repositories;
using Domain.Scheduling.Models;
using Infrastructure.Persistence;
using Infrastructure.Scheduling.Entities;
using Infrastructure.Scheduling.Mapping;
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
