using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
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

        public async Task<List<JobDetailDto>> GetAllJobDetailsAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            var jobDetails = new List<JobDetailDto>();
            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);
                var dto = new JobDetailDto
                {
                    JobKey = jobKey,
                    JobType = jobDetail.JobType,
                    Durable = jobDetail.Durable,
                    PersistJobDataAfterExecution = jobDetail.PersistJobDataAfterExecution,
                    Description = jobDetail.Description
                };

                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                if (triggers != null && triggers.Any())
                {
                    var state = await scheduler.GetTriggerState(triggers.First().Key);
                    dto.TriggerState = state;
                }

                jobDetails.Add(dto);
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