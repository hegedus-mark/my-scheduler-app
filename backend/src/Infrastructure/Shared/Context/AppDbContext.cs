using Application.Calendar.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.DTOs;
using Infrastructure.Scheduling.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Context;

public class AppDbContext : DbContext
{
    //Calendar Entities
    public DbSet<CalendarDayDto> CalendarDays;
    public DbSet<CalendarItemDto> CalendarItems;

    //Schedule Entities
    public DbSet<TaskItemDto> TaskItems;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
    }
}
