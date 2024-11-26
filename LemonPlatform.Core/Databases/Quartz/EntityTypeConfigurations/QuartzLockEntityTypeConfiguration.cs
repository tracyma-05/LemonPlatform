using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LemonPlatform.Core.Databases.Quartz.Tables;

namespace LemonPlatform.Core.Databases.Quartz.EntityTypeConfigurations;

public class QuartzLockEntityTypeConfiguration : IEntityTypeConfiguration<QuartzLock>
{
  private readonly string _prefix;

  public QuartzLockEntityTypeConfiguration(string prefix)
  {
    this._prefix = prefix;
  }

  public void Configure(EntityTypeBuilder<QuartzLock> builder)
  {
    builder.ToTable(_prefix + "LOCKS");

    builder.HasKey(x => new { x.SchedulerName, x.LockName });

    builder.Property(x => x.SchedulerName)
      .HasColumnName("SCHED_NAME")
      .HasColumnType("text")
      .IsRequired();

    builder.Property(x => x.LockName)
      .HasColumnName("LOCK_NAME")
      .HasColumnType("text")
      .IsRequired();
  }
}