using LemonPlatform.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LemonPlatform.Core
{
    public interface ILemonModule
    {
        void RegisterServices(IServiceCollection services);

        PluginItem GetMenuItem();

        void RegisterJobs(IServiceCollectionQuartzConfigurator quartz);
    }
}