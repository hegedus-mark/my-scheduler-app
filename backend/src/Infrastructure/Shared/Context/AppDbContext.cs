using Infrastructure.Calendar.Entities;
using Infrastructure.Scheduling.Configurations;
using Infrastructure.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    //Calendar Entities
    public DbSet<CalendarDayEntity> CalendarDays { get; set; }
    public DbSet<CalendarItemEntity> CalendarItems { get; set; }

    //Schedule Entities
    public DbSet<TaskItemEntity> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
    }
}
