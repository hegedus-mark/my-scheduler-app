using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<CalendarItemEntity> CalendarItems;
    public DbSet<DayEntity> Days;

    public DbContext(DbContextOptions<DbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
