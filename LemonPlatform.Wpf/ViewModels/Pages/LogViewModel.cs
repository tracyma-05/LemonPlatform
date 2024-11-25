using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class LogViewModel : ObservableObject, ISingletonDependency
    {
        public LogViewModel()
        {
            SelectedDate = DateTime.Today;
            LogLevels = GetLogLevels();
            SelectedLogLevel = LogLevels.LastOrDefault();
        }

        [ObservableProperty]
        private string? _selectedLogLevel;

        [ObservableProperty]
        private ObservableCollection<string> _logLevels;

        [ObservableProperty]
        private DateTime _selectedDate;

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            LogLevels = GetLogLevels();
            SelectedLogLevel = LogLevels.LastOrDefault();
        }

        [ObservableProperty]
        private string _logContent;

        [RelayCommand(CanExecute = nameof(CanExecute))]
        private async void Search()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            var fileName = $"{SelectedLogLevel}.{SelectedDate:yyyy-MM-dd}.log";
            var filePath = Path.Combine(path, SelectedLogLevel, fileName);

            await using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var readArr = new byte[1024 * 5];
            var count = await stream.ReadAsync(readArr, 0, readArr.Length);
            LogContent = Encoding.UTF8.GetString(readArr, 0, count);
        }

        #region private

        private ObservableCollection<string> GetLogLevels()
        {
            var result = new ObservableCollection<string>();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs.Reverse())
            {
                var logLevel = dir.Split('\\').LastOrDefault();
                if (string.IsNullOrEmpty(logLevel))
                {
                    continue;
                }

                var fileName = $"{logLevel}.{SelectedDate:yyyy-MM-dd}.log";
                var filePath = Path.Combine(dir, fileName);
                if (File.Exists(filePath))
                {
                    result.Add(logLevel);
                }
            }

            return result;
        }

        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(SelectedLogLevel);
        }

        #endregion
    }
}