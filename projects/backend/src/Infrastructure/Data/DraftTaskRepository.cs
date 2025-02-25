using Application.DraftTasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DraftTaskRepository : BaseRepository<DraftTask>, IDraftTaskRepository
{
    public DraftTaskRepository(AppDbContext context) : base(context)
    {
    }
}