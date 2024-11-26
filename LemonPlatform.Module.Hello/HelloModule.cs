using LemonPlatform.Core;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Hello.Jobs;
using LemonPlatform.Module.Hello.Views;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LemonPlatform.Module.Hello
{
    public class HelloModule : ILemonModule
    {
        public PluginItem GetMenuItem()
        {
            return new PluginItem("Hello", typeof(HelloView), "Graph", "#FAA570", "用于测试");
        }

        public void RegisterJobs(IServiceCollectionQuartzConfigurator quartz)
        {
            quartz.ScheduleJob<HelloJob>(trigger => trigger
            .WithIdentity("Combined Configuration Trigger")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.Now.AddSeconds(7)))
            .WithDailyTimeIntervalSchedule(interval: 10, intervalUnit: IntervalUnit.Second)
            .WithDescription("my awesome trigger configured for a job with single call"));
        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}