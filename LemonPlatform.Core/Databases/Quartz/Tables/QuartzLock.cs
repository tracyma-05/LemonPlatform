namespace LemonPlatform.Core.Databases.Quartz.Tables
{
    public class QuartzLock
    {
        public string SchedulerName { get; set; } = null!;
        public string LockName { get; set; } = null!;
    }
}