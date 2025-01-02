using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scheduling.Application.Contracts.Repositories;
using Scheduling.Application.DataTransfer.DTOs;

namespace Infrastructure.Scheduling.Repositories;

internal class TaskItemRepository : BaseRepository<TaskItemDto>, ITaskItemRepository
{
    public TaskItemRepository(DbContext context)
        : base(context) { }
}
