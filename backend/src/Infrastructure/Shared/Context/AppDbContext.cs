using Application.Calendar.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.DTOs;
using Infrastructure.Scheduling.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    //Calendar Entities
    public DbSet<CalendarDayDto> CalendarDays { get; set; }
    public DbSet<CalendarItemDto> CalendarItems { get; set; }

    //Schedule Entities
    public DbSet<TaskItemDto> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
    }
}
