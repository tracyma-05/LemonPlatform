using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.Core.Infrastructures.MainWindowService;
using LemonPlatform.Core.Models;
using LemonPlatform.Wpf.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace LemonPlatform.Wpf.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IRecipient<LemonMessage>, ITransientDependency
    {
        private IAsyncRelayCommand? _asyncRelayCommand;

        public MainWindowViewModel()
        {
            WeakReferenceMessenger.Default.Register(this);

            MenuItems = new ObservableCollection<LemonMenuItem>(LemonMenuItem.MenuItems);
            SelectMenuItem = MenuItems.First();
            MessageHelper.SendSnackMessage("Lemon Platform");
            MessageHelper.SendStatusBarTextMessage("Ready");
        }

        [ObservableProperty]
        private object _currentPage;

        [ObservableProperty]
        private ObservableCollection<LemonMenuItem> _menuItems;

        [ObservableProperty]
        private LemonMenuItem _selectMenuItem;

        [ObservableProperty]
        private bool _isBusy;
        partial void OnIsBusyChanged(bool oldValue, bool newValue)
        {
            if (oldValue && !newValue && _asyncRelayCommand != null && _asyncRelayCommand.IsRunning)
            {
                _asyncRelayCommand.Cancel();
                _asyncRelayCommand = null;
            }
        }

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isIndeterminate;

        partial void OnSelectMenuItemChanged(LemonMenuItem? oldValue, LemonMenuItem newValue)
        {
            CurrentPage = IocManager.Instance.ServiceProvider.GetRequiredService(SelectMenuItem.PageType);
        }

        public void Receive(LemonMessage message)
        {
            if (message.Content == null) return;
            switch (message.MessageType)
            {
                case MessageType.Menu:
                    {
                        var name = message.Content.ToString();
                        var menu = MenuItems.FirstOrDefault(x => x.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
                        if (menu == null) return;
                        SelectMenuItem = menu;

                        break;
                    }
                case MessageType.IsBusy:
                    {
                        var model = (BusyItem)message.Content;
                        _asyncRelayCommand = model.Command;
                        IsBusy = model.IsBusy;

                        break;
                    }
                case MessageType.Snack:
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            if (Application.Current.MainWindow is IMainHostWindow window)
                            {
                                window.AddSnackMessage(message.Content.ToString()!);
                            }
                        });

                        break;
                    }
                case MessageType.StatusBarText:
                    {
                        StatusMessage = message.Content.ToString();
                        break;
                    }

                case MessageType.StatusBarProcess:
                    {
                        IsIndeterminate = bool.Parse(message.Content.ToString()!);
                        break;
                    }
                default:

                    break;
            }
        }
    }
}