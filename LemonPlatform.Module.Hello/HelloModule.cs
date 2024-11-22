using LemonPlatform.Core;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Hello.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Hello
{
    public class HelloModule : ILemonModule
    {
        public ObservableCollection<PluginItem> GetMenuItems()
        {
            return new ObservableCollection<PluginItem>
            {
                new PluginItem("Hello", typeof(HelloView), "Graph", "#FAA570", "用于测试"  )
            };
        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}