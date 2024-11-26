using Quartz;

namespace LemonPlatform.Core.Models
{
    public class JobDetailDto
    {
        public JobKey JobKey { get; set; }
        public string Description { get; set; }
    }
}