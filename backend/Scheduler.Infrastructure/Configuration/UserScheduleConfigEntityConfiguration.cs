using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Application.Entities;

namespace Infrastructure.Configuration;

public class UserScheduleConfigEntityConfiguration
    : IEntityTypeConfiguration<UserScheduleConfigEntity>
{
    public void Configure(EntityTypeBuilder<UserScheduleConfigEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.WorkingDays).IsRequired();
    }
}
