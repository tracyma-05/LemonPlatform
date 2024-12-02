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
        private readonly int _defaultLogCount = 100;
        private readonly string _defaultLogLevel = "info";
        public LogViewModel()
        {
            SelectedDate = DateTime.Today;
            LogCounts = GetLogCounts();
            SelectedLogCount = LogCounts.First(x => x.Equals(_defaultLogCount));

            LogLevels = GetLogLevels();
            SelectedLogLevel = LogLevels.First(x => x.Equals(_defaultLogLevel));
        }

        [ObservableProperty]
        private string _selectedLogLevel;

        [ObservableProperty]
        private ObservableCollection<string> _logLevels;

        [ObservableProperty]
        private int _selectedLogCount;

        [ObservableProperty]
        private ObservableCollection<int> _logCounts;

        [ObservableProperty]
        private DateTime _selectedDate;

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            LogLevels = GetLogLevels();
            if (string.IsNullOrEmpty(SelectedLogLevel))
            {
                SelectedLogLevel = LogLevels.First(x => x.Equals(_defaultLogLevel));
            }
        }

        [ObservableProperty]
        private string _logContent;

        [RelayCommand(CanExecute = nameof(CanExecute))]
        private async Task Search(CancellationToken token)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            var fileName = $"{SelectedLogLevel}.{SelectedDate:yyyy-MM-dd}.log";
            var filePath = Path.Combine(path, SelectedLogLevel, fileName);
            if (SelectedLogCount > 0)
            {
                LogContent = await ReadLastLinesAsync(filePath, SelectedLogCount);
            }
            else
            {
                LogContent = await ReadLogFileAsync(filePath, token);
            }
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

        private ObservableCollection<int> GetLogCounts()
        {
            return new ObservableCollection<int>
            {
                100,
                200,
                500,
                1000,
                -1
            };
        }

        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(SelectedLogLevel);
        }

        private async Task<string> ReadLastLinesAsync(string filePath, int lineCount)
        {
            var lines = new List<string>();
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            long position = 0;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                position = stream.Length;
                while (lines.Count < lineCount && position > 0)
                {
                    int toRead = (int)Math.Min(bufferSize, position);
                    position -= toRead;
                    stream.Seek(position, SeekOrigin.Begin);

                    bytesRead = await stream.ReadAsync(buffer, 0, toRead);
                    for (int i = bytesRead - 1; i >= 0; i--)
                    {
                        if (buffer[i] == '\n')
                        {
                            var line = Encoding.UTF8.GetString(buffer, i + 1, bytesRead - i - 1);
                            lines.Insert(0, line.TrimEnd('\r', '\n'));
                            bytesRead = i;
                            if (lines.Count == lineCount)
                                break;
                        }
                    }

                    if (bytesRead > 0)
                    {
                        var remaining = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (!string.IsNullOrWhiteSpace(remaining))
                        {
                            lines.Insert(0, remaining.TrimEnd('\r', '\n'));
                        }
                    }
                }
            }

            return string.Join(Environment.NewLine, lines);
        }

        public async Task<string> ReadLogFileAsync(string filePath, CancellationToken token)
        {
            var stringBuilder = new StringBuilder();

            await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        stringBuilder.AppendLine(line);
                    }

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}