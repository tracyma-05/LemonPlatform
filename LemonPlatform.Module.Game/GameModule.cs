using LemonPlatform.Core;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Game.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Module.Game
{
    public class GameModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("Mine Sweeper", typeof(MineSweeperView), "Mine", "#edd1d8", "mine sweeper", PluginType.Games),
                new PluginItem("Puzzle", typeof(APuzzleADayView), "Puzzle", "#cca4e3", "a puzzle a day", PluginType.Games),
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