using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Core;
using Microsoft.Extensions.DependencyInjection;
using LemonPlatform.Module.Game.Views;

namespace LemonPlatform.Module.Game
{
    public class GameModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("Mine Sweeper", typeof(MineSweeperView), "Mine", "#052E24", "mine sweeper", PluginType.Games),
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