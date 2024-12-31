using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Scheduler.Application.Entities;

namespace Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<DayEntity> Days { get; set; }
    public DbSet<CalendarItemEntity> CalendarItems { get; set; }
    public DbSet<RecurrencePatternEntity> RecurrencePatterns { get; set; }
    public DbSet<UserScheduleConfigEntity> UserScheduleConfigs { get; set; }
    public DbSet<DayScheduleOverrideEntity> DayScheduleOverrides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DayEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CalendarItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RecurrencePatternEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserScheduleConfigEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DayScheduleOverrideEntityConfiguration());
    }
}
