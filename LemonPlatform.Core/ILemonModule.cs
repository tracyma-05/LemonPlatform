using LemonPlatform.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LemonPlatform.Core
{
    public interface ILemonModule
    {
        void RegisterServices(IServiceCollection services);

        ObservableCollection<PluginItem> GetMenuItems();
    }
}