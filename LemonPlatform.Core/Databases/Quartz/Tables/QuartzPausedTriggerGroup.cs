namespace LemonPlatform.Core.Databases.Quartz.Tables
{
    public class QuartzPausedTriggerGroup
    {
        public string SchedulerName { get; set; } = null!;
        public string TriggerGroup { get; set; } = null!;
    }
}