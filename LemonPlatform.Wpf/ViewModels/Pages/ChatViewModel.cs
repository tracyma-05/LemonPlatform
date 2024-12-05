using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Databases;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Wpf.Configs;
using LemonPlatform.Wpf.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class ChatViewModel : ObservableObject, IRecipient<LemonMessage>, ISingletonDependency
    {
        private readonly LemonDbContext _lemonDbContext;
        private readonly ChatEmptyView _chatEmptyView;
        public ChatViewModel(LemonDbContext lemonDbContext, ChatEmptyView chatEmptyView)
        {
            WeakReferenceMessenger.Default.Register(this);
            _lemonDbContext = lemonDbContext;
            _chatEmptyView = chatEmptyView;

            ChatItems = new ObservableCollection<PluginItem>(LemonConstants.ChatItems);
            SelectedChatItem = LemonConstants.SelectChatItem ?? ChatItems.FirstOrDefault();

            if (SelectedChatItem == null)
            {
                CurrentChat = chatEmptyView;
            }
        }

        [ObservableProperty]
        private ObservableCollection<PluginItem> _chatItems;

        [ObservableProperty]
        private PluginItem? _selectedChatItem;
        partial void OnSelectedChatItemChanged(PluginItem? oldValue, PluginItem? newValue)
        {
            if (newValue == null)
            {
                CurrentChat = _chatEmptyView;
                return;
            };

            CurrentChat = newValue.Content;
            UpdateChatConfig();
        }

        [ObservableProperty]
        private object _currentChat;

        [ObservableProperty]
        private string _searchKeyword;

        [RelayCommand]
        private void ScreenShot(Frame control)
        {
            ScreenShotHelper.ScreenShot(control);
        }

        [RelayCommand]
        private void Remove()
        {
            ChatItems.Remove(SelectedChatItem);
            SelectedChatItem = ChatItems.FirstOrDefault();

            UpdateChatConfig();
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
            UpdateChatConfig();
        }

        private async void UpdateChatConfig()
        {
            var chatPreference = await _lemonDbContext.UserPreference.FirstOrDefaultAsync(x => x.Id == LemonConstants.ChatConfigId);
            if (chatPreference == null) return;
            var items = ChatItems.Select(x => x.Guid).ToList();
            var chatConfig = new ChatConfig
            {
                Items = string.Join(',', items),
                SelectItem = SelectedChatItem?.Guid,
            };

            chatPreference.Content = JsonSerializer.Serialize(chatConfig);
            chatPreference.LastModifiedAt = DateTime.Now;
            await _lemonDbContext.SaveChangesAsync();
        }
    }
}