using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Core;
using Microsoft.Extensions.DependencyInjection;
using LemonPlatform.Module.Tools.Views;

namespace LemonPlatform.Module.Tools
{
    public class ToolsModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem> { new PluginItem("Compare", typeof(CompareToolView), "FileCompare", "#1DE9B6", "string compare", PluginType.TextTools) };
        }

        public void PostInit(IServiceProvider serviceProvider)
        {

        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}