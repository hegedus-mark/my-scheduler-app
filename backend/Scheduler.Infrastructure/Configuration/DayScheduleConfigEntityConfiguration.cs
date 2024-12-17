using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class DayScheduleOverrideEntityConfiguration
    : IEntityTypeConfiguration<DayScheduleOverrideEntity>
{
    public void Configure(EntityTypeBuilder<DayScheduleOverrideEntity> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Date).IsRequired();

        builder.HasIndex(d => d.Date).IsUnique();
    }
}
