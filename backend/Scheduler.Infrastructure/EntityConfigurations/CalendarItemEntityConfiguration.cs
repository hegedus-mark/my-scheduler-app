using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class CalendarItemEntityConfiguration : IEntityTypeConfiguration<CalendarItemEntity>
{
    public void Configure(EntityTypeBuilder<CalendarItemEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ItemType).IsRequired().HasMaxLength(20);

        builder
            .HasOne(c => c.RecurrencePattern)
            .WithOne(r => r.CalendarItem)
            .HasForeignKey<RecurrencePatternEntity>(r => r.CalendarItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new
        {
            c.DayId,
            c.StartTime,
            c.EndTime,
        });
    }
}
