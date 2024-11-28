using Quartz;

namespace LemonPlatform.Core.Models
{
    public class JobDetailDto
    {
        public JobKey JobKey { get; set; }
        public string JobType { get; set; }
        public TriggerState TriggerState { get; set; }
        public bool Durable { get; set; }
        public bool PersistJobDataAfterExecution { get; set; }
        public string Description { get; set; }
    }
}