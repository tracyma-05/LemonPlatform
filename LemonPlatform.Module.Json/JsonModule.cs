using LemonPlatform.Core;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Json.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Module.Json
{
    public class JsonModule : ILemonModule
    {
        public PluginItem GetMenuItem()
        {
            return new PluginItem("Json", typeof(JsonView), "CodeJson", "#D9D9D9", "Json转换器");
        }

        public void PostInit(IServiceProvider serviceProvider)
        {
            
        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}