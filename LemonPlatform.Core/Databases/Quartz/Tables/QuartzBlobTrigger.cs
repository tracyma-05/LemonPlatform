using LemonPlatform.Core.Databases.Quartz.Tables;

namespace LemonPlatform.Core.Databases.Quartz.Tables
{
    public class QuartzBlobTrigger
    {
        public string SchedulerName { get; set; } = null!;
        public string TriggerName { get; set; } = null!;
        public string TriggerGroup { get; set; } = null!;
        public byte[]? BlobData { get; set; }

        public QuartzTrigger Trigger { get; set; } = null!;
    }
}