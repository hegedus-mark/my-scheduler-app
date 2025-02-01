using Infrastructure.Calendar.Entities;
using Infrastructure.Scheduling.Configurations;
using Infrastructure.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Context;

/// <summary>
///     The main database context for the application, responsible for managing database connections
///     and entity configurations using Entity Framework Core.
/// </summary>
/// <remarks>
///     <para>
///         This context handles two main feature areas:
///         1. Calendar Management - Manages calendar days and calendar items
///         2. Task Scheduling - Handles scheduled tasks and their configurations
///     </para>
///     <para>
///         Key Concepts:
///         - DbContext is the primary class that coordinates Entity Framework functionality
///         - DbSet properties represent tables in the database
///         - Entity configurations define how domain models map to database tables
///         - The context manages database operations
///     </para>
/// </remarks>
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
