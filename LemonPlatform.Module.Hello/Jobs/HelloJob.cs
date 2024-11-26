using LemonPlatform.Core.Services;
using Quartz;
using System.Diagnostics;

namespace LemonPlatform.Module.Hello.Jobs
{
    public class HelloJob : IJob
    {
        private readonly JobService _jobService;

        public HelloJob(JobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual async Task Execute(IJobExecutionContext context)
        {
            var jobs = await _jobService.GetAllJobDetailsAsync();
            Debug.WriteLine($"{jobs.Count}");
        }
    }
}