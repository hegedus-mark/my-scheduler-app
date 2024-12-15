using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

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
