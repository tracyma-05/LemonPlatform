using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Wpf.Views.Pages;
using MaterialDesignThemes.Wpf;

namespace LemonPlatform.Wpf.Models
{
    [ObservableObject]
    public partial class LemonMenuItem
    {
        [ObservableProperty]
        public string _title;

        [ObservableProperty]
        public PackIconKind _selectedIcon;

        [ObservableProperty]
        public PackIconKind _unselectedIcon;

        [ObservableProperty]
        private object? _notification;

        [ObservableProperty]
        private Type _pageType;

        public static IEnumerable<LemonMenuItem> MenuItems
        {
            get => new List<LemonMenuItem>()
            {
                new LemonMenuItem
                {
                    Title = "Home",
                    SelectedIcon = PackIconKind.Home,
                    UnselectedIcon = PackIconKind.HomeOutline,
                    PageType = typeof(HomeView)
                },
                new LemonMenuItem
                {
                    Title = "Chat",
                    SelectedIcon = PackIconKind.ChatProcessing,
                    UnselectedIcon = PackIconKind.ChatProcessingOutline,
                    PageType = typeof(ChatView)
                },
                new LemonMenuItem
                {
                    Title = "Plugin",
                    SelectedIcon = PackIconKind.GamepadSquare,
                    UnselectedIcon = PackIconKind.GamepadSquareOutline,
                    PageType = typeof(PluginView)
                },
                new LemonMenuItem
                {
                    Title = "Setting",
                    SelectedIcon = PackIconKind.CogBox,
                    UnselectedIcon = PackIconKind.Cog,
                    PageType = typeof(SettingView)
                },
                new LemonMenuItem
                {
                    Title = "Log",
                    SelectedIcon = PackIconKind.Post,
                    UnselectedIcon = PackIconKind.PostOutline,
                    PageType = typeof(LogView)
                },
            };
        }
    }
}