using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Infrastructures.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

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
            PluginType pluginType,
            UserRole role = UserRole.SuperAdmin | UserRole.Administrator | UserRole.NormalUser)
        {
            Name = name;
            _contentType = contentType;
            Role = role;
            IconFont = iconFont;
            BackgroundColor = backgroundColor;
            Description = description;
            PluginType = pluginType;
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

        [ObservableProperty]
        private PluginType _pluginType;

        public string Guid => _contentType.GUID.ToString();

        public object Content => CreateContent();

        private object CreateContent()
        {
            var view = IocManager.Instance.ServiceProvider.GetRequiredService(_contentType);
            if (view is Page control)
            {
                if (control.DataContext is null)
                {
                    var contextNamespace = _contentType.Namespace?.Replace("Views", "ViewModels");
                    var viewModelName = $"{contextNamespace}.{_contentType.Name}Model";
                    var viewModelType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).FirstOrDefault(type => type.FullName == viewModelName);
                    if (viewModelType != null)
                    {
                        var viewModel = IocManager.Instance.ServiceProvider.GetService(viewModelType);
                        if (viewModel != null)
                        {
                            control.DataContext = viewModel;
                        }
                    }

                }
            }

            return view;
        }
    }
}