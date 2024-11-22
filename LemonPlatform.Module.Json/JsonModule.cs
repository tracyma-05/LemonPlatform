using LemonPlatform.Core;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Json.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Json
{
    public class JsonModule : ILemonModule
    {
        public ObservableCollection<PluginItem> GetMenuItems()
        {
            return new ObservableCollection<PluginItem>
            {
                new PluginItem("Json", typeof(JsonView), "CodeJson", "#D9D9D9", "Json转换器")
            };
        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}