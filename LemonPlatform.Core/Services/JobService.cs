using LemonPlatform.Core.Infrastructures.Denpendency;
using Quartz;
using Quartz.Impl.Matchers;

namespace LemonPlatform.Core.Services
{
    public class JobService : ITransientDependency
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public JobService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<List<IJobDetail>> GetAllJobDetailsAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            var jobDetails = new List<IJobDetail>();
            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);
                jobDetails.Add(jobDetail);
            }

            return jobDetails;
        }

        public async Task StartJobAsync(JobKey jobKey)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.TriggerJob(jobKey);
        }

        public async Task PauseJobAsync(JobKey jobKey)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.PauseJob(jobKey);
        }

        public async Task ResumeJobAsync(JobKey jobKey)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.ResumeJob(jobKey);
        }

        public async Task DeleteJobAsync(JobKey jobKey)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.DeleteJob(jobKey);
        }
    }
}