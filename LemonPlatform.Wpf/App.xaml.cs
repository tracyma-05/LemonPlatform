using Hardcodet.Wpf.TaskbarNotification;
using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Databases;
using LemonPlatform.Core.Databases.Models;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.Wpf.Configs;
using LemonPlatform.Wpf.Exceptions;
using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.Resources;
using LemonPlatform.Wpf.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using NLog.Extensions.Logging;
using System.IO;
using System.Text.Json;
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

            var splashScreen = new SplashScreen("Resources/Images/lemon.png");
            splashScreen.Show(true, true);

            await _host.StartAsync();

            var isDark = await GetThemeAsync();
            ThemeHelper.SetLemonTheme(isDark);
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            var handler = _host.Services.GetRequiredService<LemonExceptionHandler>();
            ExceptionHandler(handler);

            var dbContext = _host.Services.GetRequiredService<LemonDbContext>();
            InitializeDatabase(dbContext);

            var login = _host.Services.GetRequiredService<MainWindow>();
            login.Show();

            var notifyIconDataContext = _host.Services.GetRequiredService<NotifyIconViewModel>();
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            notifyIcon.DataContext = notifyIconDataContext;

            PostInit();

            await SetHistoryChatAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            notifyIcon.Dispose();
            base.OnExit(e);
        }

        private async Task<bool> GetThemeAsync()
        {
            var isDark = SystemThemeHelper.GetWindowsTheme();
            var context = _host.Services.GetRequiredService<LemonDbContext>();
            var theme = await context.UserPreference.FirstOrDefaultAsync(x => x.Id == LemonConstants.ThemeConfigId);
            if (theme != null)
            {
                var config = JsonSerializer.Deserialize<ThemeConfig>(theme.Content);
                isDark = config.IsDarkTheme;
            }

            return isDark;
        }

        private async Task SetHistoryChatAsync()
        {
            var context = _host.Services.GetRequiredService<LemonDbContext>();
            var chat = await context.UserPreference.FirstOrDefaultAsync(x => x.Id == LemonConstants.ChatConfigId);
            var type = GetType();
            var dtNow = DateTime.Now;
            if (chat == null)
            {
                chat = new UserPreference
                {
                    Id = LemonConstants.ChatConfigId,
                    Content = string.Empty,
                    UserId = LemonConstants.GuestUserId,
                    LastModifiedAt = dtNow,
                    CreatedAt = dtNow,
                    ModuleName = $"{type.Module.Name}-LemonPlatform.Wpf.ViewModels.Pages-ChatViewModel"
                };

                await context.UserPreference.AddAsync(chat);
            }
            else
            {
                if (string.IsNullOrEmpty(chat.Content)) return;
                var chatConfig = JsonSerializer.Deserialize<ChatConfig>(chat.Content);
                if (chatConfig == null || string.IsNullOrEmpty(chatConfig.Names)) return;
                var names = chatConfig.Names.Split(',');
                var pages = LemonConstants.PageItems.Select(x => x.Name).ToList();
                foreach (var item in LemonConstants.PageItems)
                {
                    if (names.Contains(item.Name))
                    {
                        LemonConstants.ChatItems.Add(item);
                    }
                }

                if (chatConfig.SelectName != null && !pages.Contains(chatConfig.SelectName))
                {
                    chatConfig.SelectName = null;
                }

                if (chatConfig.SelectName != null)
                {
                    LemonConstants.SelectChatItem = LemonConstants.ChatItems.FirstOrDefault(x => x.Name == chatConfig.SelectName);
                }
            }

            await context.SaveChangesAsync();
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

        private void InitializeDatabase(LemonDbContext context)
        {
            var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lemon.db");
            try
            {
                var isCreated = context.Database.EnsureCreated();
                if (isCreated)
                {
                    context.Database.Migrate();
                }
            }
            catch
            {
                //ignore
            }
        }

        private void PostInit()
        {
            var provider = IocManager.Instance.ServiceProvider.GetRequiredService<IServiceProvider>();
            foreach (var item in LemonConstants.Modules)
            {
                try
                {
                    item.PostInit(provider);
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}