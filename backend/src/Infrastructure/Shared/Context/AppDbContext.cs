using Application.Calendar.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Context;

public class AppDbContext : DbContext
{
    //Calendar Dtos
    public DbSet<CalendarDayDto> CalendarDays;
    public DbSet<CalendarItemDto> CalendarItems;

    //Schedule Dtos
    public DbSet<TaskItemDto> TaskItems;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
