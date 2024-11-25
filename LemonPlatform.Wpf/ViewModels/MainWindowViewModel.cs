using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.Core.Infrastructures.Messages;
using LemonPlatform.Core.Models;
using LemonPlatform.Wpf.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LemonPlatform.Wpf.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IRecipient<LemonMessage>, ITransientDependency
    {
        private readonly IMessageService _messageService;
        public MainWindowViewModel(IMessageService messageService)
        {
            WeakReferenceMessenger.Default.Register(this);

            MenuItems = new ObservableCollection<LemonMenuItem>(LemonMenuItem.MenuItems);
            SelectMenuItem = MenuItems.First();
            _messageService = messageService;

            _messageService.ShowSnackMessage("Lemon Platform");
        }

        [ObservableProperty]
        private object _currentPage;

        [ObservableProperty]
        private ObservableCollection<LemonMenuItem> _menuItems;

        [ObservableProperty]
        private LemonMenuItem _selectMenuItem;

        partial void OnSelectMenuItemChanged(LemonMenuItem? oldValue, LemonMenuItem newValue)
        {
            CurrentPage = IocManager.Instance.ServiceProvider.GetRequiredService(SelectMenuItem.PageType);
        }

        public void Receive(LemonMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.Menu:
                    {
                        if (message.Content == null) return;
                        var name = message.Content.ToString();
                        var menu = MenuItems.FirstOrDefault(x=>x.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
                        if(menu == null) return;
                        SelectMenuItem = menu;
                        
                        break;
                    }
                default:
                    
                    break;
            }
        }
    }
}