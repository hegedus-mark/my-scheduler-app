using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Application.Entities;

namespace Infrastructure.Configuration;

public class RecurrencePatternEntityConfiguration
    : IEntityTypeConfiguration<RecurrencePatternEntity>
{
    public void Configure(EntityTypeBuilder<RecurrencePatternEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Type).IsRequired();

        builder.Property(r => r.Interval).IsRequired();
    }
}
