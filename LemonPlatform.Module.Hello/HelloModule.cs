using LemonPlatform.Core;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Hello.Jobs;
using LemonPlatform.Module.Hello.Views;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LemonPlatform.Module.Hello
{
    public class HelloModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem> { new PluginItem("Hello", typeof(HelloView), "Graph", "#FAA570", "用于测试", PluginType.Else) };
        }

        public async void PostInit(IServiceProvider serviceProvider)
        {
            var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            var jobKey = new JobKey("HelloJob", "DEFAULT");
            var triggerKey = new TriggerKey("Combined Configuration Trigger", "DEFAULT");

            var jobExists = await scheduler.CheckExists(jobKey);
            var triggerExists = await scheduler.CheckExists(triggerKey);

            if (!jobExists && !triggerExists)
            {
                // 定义作业
                var job = JobBuilder.Create<HelloJob>()
                    .WithIdentity(jobKey)
                    .WithDescription("My awesome job")
                    .Build();

                // 定义触发器
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("Combined Configuration Trigger")
                    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.Now.AddSeconds(7)))
                    .WithDailyTimeIntervalSchedule(x => x
                        .WithIntervalInSeconds(10))
                    .WithDescription("My awesome trigger configured for a job with single call")
                    .Build();

                // 调度作业
                await scheduler.ScheduleJob(job, trigger);
            }
        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}