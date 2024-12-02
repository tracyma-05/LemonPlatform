using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class HomeView : UserControl, ITransientDependency
    {
        public HomeView(HomeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Resources["PopupExample"] is Popup popup)
            {
                popup.PlacementTarget = button;
                popup.IsOpen = true;
            }
        }
    }
}