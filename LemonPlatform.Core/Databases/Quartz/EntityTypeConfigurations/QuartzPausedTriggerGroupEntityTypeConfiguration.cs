using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LemonPlatform.Core.Databases.Quartz.Tables;

namespace LemonPlatform.Core.Databases.Quartz.EntityTypeConfigurations;

public class QuartzPausedTriggerGroupEntityTypeConfiguration : IEntityTypeConfiguration<QuartzPausedTriggerGroup>
{
    private readonly string _prefix;

    public QuartzPausedTriggerGroupEntityTypeConfiguration(string prefix)
    {
        this._prefix = prefix;
    }

    public void Configure(EntityTypeBuilder<QuartzPausedTriggerGroup> builder)
    {
        builder.ToTable(_prefix + "PAUSED_TRIGGER_GRPS");

        builder.HasKey(x => new { x.SchedulerName, x.TriggerGroup });

        builder.Property(x => x.SchedulerName)
          .HasColumnName("SCHED_NAME")
          .HasColumnType("text")
          .IsRequired();

        builder.Property(x => x.TriggerGroup)
          .HasColumnName("TRIGGER_GROUP")
          .HasColumnType("text")
          .IsRequired();
    }
}