using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Wpf.Commons;
using System.Collections.ObjectModel;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class PluginViewModel : ObservableObject, ITransientDependency
    {
        public PluginViewModel()
        {
            PluginItems = new ObservableCollection<PluginItem>(LemonConstants.PageItems);
        }

        [ObservableProperty]
        private ObservableCollection<PluginItem> _pluginItems;

        [ObservableProperty]
        private PluginItem _selectedPluginItem;
        partial void OnSelectedPluginItemChanged(PluginItem? oldValue, PluginItem newValue)
        {
            if (newValue == null) return;
            MessageHelper.SendLemonMessage(MessageType.Menu, "Chat");
            MessageHelper.SendLemonMessage(MessageType.Plugin, newValue);
        }
    }
}