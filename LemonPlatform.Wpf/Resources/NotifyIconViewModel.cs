using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows;

namespace LemonPlatform.Wpf.Resources
{
    public partial class NotifyIconViewModel : ObservableObject, ISingletonDependency
    {
        [RelayCommand]
        private void ShowWindow()
        {
            var window = Application.Current.MainWindow;
            if (window is { WindowState: WindowState.Minimized })
            {
                window.WindowState = WindowState.Normal;
            }
        }

        [RelayCommand]
        private void HideWindow()
        {
            var window = Application.Current.MainWindow;
            if (window.WindowState == WindowState.Normal || window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        [RelayCommand]
        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}