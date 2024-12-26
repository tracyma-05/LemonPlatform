using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class HomeView : UserControl, ITransientDependency
    {
        private const string GitHubUrl = "https://github.com/tracyma-05/LemonPlatform";
        private const string EmailUrl = "mailto://zhongbin_ma@outlook.com";
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

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e) => LinkHelper.OpenInBrowser(GitHubUrl);

        private void EmailButton_OnClick(object sender, RoutedEventArgs e) => LinkHelper.OpenInBrowser(EmailUrl);
    }
}