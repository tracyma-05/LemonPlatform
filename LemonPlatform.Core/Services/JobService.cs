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
                    JobType = jobDetail.JobType.FullName,
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

        public async Task StartJobAsync(JobKey jobKey, CancellationToken token)
        {
            var scheduler = await _schedulerFactory.GetScheduler(token);
            await scheduler.TriggerJob(jobKey, token);
        }

        public async Task PauseJobAsync(JobKey jobKey, CancellationToken token)
        {
            var scheduler = await _schedulerFactory.GetScheduler(token);
            await scheduler.PauseJob(jobKey, token);
        }

        public async Task ResumeJobAsync(JobKey jobKey, CancellationToken token)
        {
            var scheduler = await _schedulerFactory.GetScheduler(token);
            await scheduler.ResumeJob(jobKey, token);
        }

        public async Task DeleteJobAsync(JobKey jobKey, CancellationToken token)
        {
            var scheduler = await _schedulerFactory.GetScheduler(token);
            await scheduler.DeleteJob(jobKey, token);
        }
    }
}