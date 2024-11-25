using Hardcodet.Wpf.TaskbarNotification;
using LemonPlatform.Wpf.Exceptions;
using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.Resources;
using LemonPlatform.Wpf.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using NLog.Extensions.Logging;
using System.Windows;

namespace LemonPlatform.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;
        private TaskbarIcon notifyIcon;

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
                    logging.ClearProviders();
                    logging.AddNLog(context.Configuration);
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            AutoStartHelper.SetMeAutoStart();
            ApplicationHelper.CheckApplicationMutex();

            await _host.StartAsync();

            ThemeHelper.SetLemonTheme();
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            var handler = _host.Services.GetRequiredService<LemonExceptionHandler>();
            ExceptionHandler(handler);

            var login = _host.Services.GetRequiredService<MainWindow>();
            login.Show();

            var notifyIconDataContext = _host.Services.GetRequiredService<NotifyIconViewModel>();
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            notifyIcon.DataContext = notifyIconDataContext;
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            notifyIcon.Dispose();
            base.OnExit(e);
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                ThemeHelper.SetPrimaryColor();
            }
        }

        private void ExceptionHandler(LemonExceptionHandler handler)
        {
            DispatcherUnhandledException += handler.ApplicationExceptionHandler;
            TaskScheduler.UnobservedTaskException += handler.UnobservedTaskExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += handler.DomainExceptionHandler;
        }
    }
}