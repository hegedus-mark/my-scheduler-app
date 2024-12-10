using Microsoft.EntityFrameworkCore;
using Scheduler.Core.Models;

namespace Infrastructure.Context;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Day> Days { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<UserConfig> UserConfigs { get; set; }

    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}