using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Application.Entities;

namespace Infrastructure.Configuration;

public class DayEntityConfiguration : IEntityTypeConfiguration<DayEntity>
{
    public void Configure(EntityTypeBuilder<DayEntity> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Date).IsRequired();

        builder.HasIndex(d => d.Date).IsUnique();

        builder
            .HasMany(d => d.CalendarItems)
            .WithOne(c => c.Day)
            .HasForeignKey(c => c.DayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
