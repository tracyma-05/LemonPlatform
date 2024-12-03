using LemonPlatform.Core.Models;
using LemonPlatform.Core;
using Microsoft.Extensions.DependencyInjection;
using LemonPlatform.Core.Enums;
using LemonPlatform.Module.Visualization.Views;

namespace LemonPlatform.Module.Visualization
{
    public class VisualizationModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("Snow Flake", typeof(SnowFlakeView), "Snowflake", "#FAA570", "fractal drawing with snow flake.", PluginType.Visualization),
            };
        }

        public void PostInit(IServiceProvider serviceProvider)
        {

        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}