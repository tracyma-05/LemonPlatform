using LemonPlatform.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Core
{
    public interface ILemonModule
    {
        void RegisterServices(IServiceCollection services);

        List<PluginItem> GetMenuItems();

        void PostInit(IServiceProvider serviceProvider);
    }
}