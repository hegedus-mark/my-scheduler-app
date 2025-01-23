using Infrastructure.Scheduling.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Scheduling.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItemEntity>
{
    public void Configure(EntityTypeBuilder<TaskItemEntity> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);

        builder.Property(t => t.DueDate).IsRequired();

        builder.Property(t => t.PriorityLevel).IsRequired().HasConversion<string>();

        builder.Property(t => t.TaskItemStatus).IsRequired().HasConversion<string>();

        builder.Property(t => t.StartDate).IsRequired(false);

        builder.Property(t => t.EndDate).IsRequired(false);

        builder.Property(t => t.FailureReason).IsRequired(false).HasMaxLength(1000);

        builder.HasIndex(t => t.TaskItemStatus);

        builder.HasIndex(t => t.DueDate);

        builder.OwnsOne(
            x => x.Duration,
            d =>
            {
                d.Property(p => p.Days).HasColumnName("DurationDays");
                d.Property(p => p.Hours).HasColumnName("DurationHours");
                d.Property(p => p.Minutes).HasColumnName("DurationMinutes");
            }
        );

        builder
            .HasIndex(t => new { t.StartDate, t.EndDate })
            .HasFilter("[StartDate] IS NOT NULL AND [EndDate] IS NOT NULL");
    }
}
