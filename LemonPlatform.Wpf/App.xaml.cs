using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System.Windows;

namespace LemonPlatform.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices(WpfModule.ConfigureServices)
                .ConfigureLogging((context, logging) =>
                {
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host.StartAsync();

            ThemeHelper.SetLemonTheme();
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            var login = _host.Services.GetRequiredService<MainWindow>();
            login.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            base.OnExit(e);
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                ThemeHelper.SetPrimaryColor();
            }
        }
    }
}