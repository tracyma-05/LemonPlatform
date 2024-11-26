using LemonPlatform.Core.Databases.Quartz.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LemonPlatform.Core.Databases.Quartz.EntityTypeConfigurations;

public class QuartzBlobTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzBlobTrigger>
{
    private readonly string _prefix;

    public QuartzBlobTriggerEntityTypeConfiguration(string prefix)
    {
        _prefix = prefix;
    }

    public void Configure(EntityTypeBuilder<QuartzBlobTrigger> builder)
    {
        builder.ToTable(_prefix + "BLOB_TRIGGERS");

        builder.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

        builder.Property(x => x.SchedulerName)
          .HasColumnName("SCHED_NAME")
          .HasColumnType("text")
          .IsRequired();

        builder.Property(x => x.TriggerName)
          .HasColumnName("TRIGGER_NAME")
          .HasColumnType("text")
          .IsRequired();

        builder.Property(x => x.TriggerGroup)
          .HasColumnName("TRIGGER_GROUP")
          .HasColumnType("text")
          .IsRequired();

        builder.Property(x => x.BlobData)
          .HasColumnName("BLOB_DATA")
          .HasColumnType("bytea");

        builder.HasOne(x => x.Trigger)
          .WithMany(x => x.BlobTriggers)
          .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
          .OnDelete(DeleteBehavior.Cascade);
    }
}