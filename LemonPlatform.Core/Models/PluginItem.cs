using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Infrastructures.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Core.Models
{
    public partial class PluginItem : ObservableObject
    {
        private readonly Type _contentType;
        public PluginItem(
            string name,
            Type contentType,
            string iconFont,
            string backgroundColor,
            string description,
            UserRole role = UserRole.SuperAdmin | UserRole.Administrator | UserRole.NormalUser)
        {
            Name = name;
            _contentType = contentType;
            Role = role;
            IconFont = iconFont;
            BackgroundColor = backgroundColor;
            Description = description;
        }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private UserRole _role;

        [ObservableProperty]
        private string _iconFont;

        [ObservableProperty]
        private string _backgroundColor;

        [ObservableProperty]
        private string _description;

        public object Content => CreateContent();

        private object CreateContent()
        {
            return IocManager.Instance.ServiceProvider.GetRequiredService(_contentType);
        }
    }
}