using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Scheduler.Core.Models;

namespace Infrastructure.Context;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<DayEntity> Days;
    public DbSet<CalendarItemEntity> CalendarItems;

    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}