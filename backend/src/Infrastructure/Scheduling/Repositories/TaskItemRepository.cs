using Application.Scheduling.Contracts.Repositories;
using Application.Scheduling.DataTransfer.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Scheduling.Repositories;

internal class TaskItemRepository : BaseRepository<TaskItemDto>, ITaskItemRepository
{
    public TaskItemRepository(DbContext context)
        : base(context) { }
}
