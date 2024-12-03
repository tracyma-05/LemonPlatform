using LemonPlatform.Core;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Graph.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Module.Graph
{
    public class GraphModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("Graph", typeof(GraphView), "Graph", "#FAA570", "graph learning", PluginType.Graph),
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