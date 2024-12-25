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
            return new List<PluginItem>
            { 
                new PluginItem("Compare", typeof(CompareToolView), "FileCompare", "#ff461f", "string compare", PluginType.TextTools),
                new PluginItem("JsonExtract", typeof(JsonExtractToolView), "CloudBraces", "#ff2d51", "json extract tool", PluginType.TextTools),
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