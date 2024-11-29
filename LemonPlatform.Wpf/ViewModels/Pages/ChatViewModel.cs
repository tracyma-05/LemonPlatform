using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class ChatViewModel : ObservableObject, IRecipient<LemonMessage>, ISingletonDependency
    {
        public ChatViewModel()
        {
            WeakReferenceMessenger.Default.Register(this);
            ChatItems = new ObservableCollection<PluginItem>();
        }

        [ObservableProperty]
        private ObservableCollection<PluginItem> _chatItems;

        [ObservableProperty]
        private PluginItem _selectedChatItem;
        partial void OnSelectedChatItemChanged(PluginItem? oldValue, PluginItem newValue)
        {
            CurrentChat = newValue.Content;
        }

        [ObservableProperty]
        private object _currentChat;

        [ObservableProperty]
        private string _searchKeyword;

        [RelayCommand]
        private void ScreenShot(Control control)
        {
            ScreenShotHelper.ScreenShot(control);
        }

        public void Receive(LemonMessage message)
        {
            if (message.MessageType != MessageType.Plugin) return;
            if (message == null) return;
            var item = message.Content as PluginItem;
            if (item == null) return;

            if (!ChatItems.Contains(item))
            {
                ChatItems.Add(item);
            }

            SelectedChatItem = item;
        }
    }
}