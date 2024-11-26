namespace LemonPlatform.Core.Databases.Quartz.Tables
{
    public class QuartzCalendar
    {
        public string SchedulerName { get; set; } = null!;
        public string CalendarName { get; set; } = null!;
        public byte[] Calendar { get; set; } = null!;
    }
}