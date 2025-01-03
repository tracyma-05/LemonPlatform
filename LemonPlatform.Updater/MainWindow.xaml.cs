using LemonPlatform.Updater.Helpers;
using LemonPlatform.Updater.Models;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace LemonPlatform.Updater
{
    public partial class MainWindow : Window
    {
        public MainWindow(List<UpdateModel> models)
        {
            InitializeComponent();
            StartUpdateProcess(models);
        }

        private async void StartUpdateProcess(List<UpdateModel> models)
        {
            await Task.Run(() =>
            {
                LogMessage("Start update...");
                var tasks = models.OrderBy(x => x.Order);
                foreach (var item in tasks)
                {
                    using var archive = ZipFile.OpenRead(item.Source);
                    var entries = archive.Entries;
                    int totalEntries = archive.Entries.Count;
                    int processedEntries = 0;

                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (!entry.IsDirectory())
                        {
                            var parts = entry.FullName.Split('/');
                            if (parts.Length > 1)
                            {
                                var fileName = string.Join("\\", parts, 1, parts.Length - 1);

                                var filePath = Path.Combine(item.Target, fileName);
                                var parentDirectory = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(parentDirectory))
                                {
                                    Directory.CreateDirectory(parentDirectory);
                                }

                                entry.ExtractToFile(filePath, true);
                            }
                        }

                        processedEntries++;
                        UpdateProgress(processedEntries, totalEntries);
                        LogMessage($"Handle: {entry.FullName}");
                    }
                }

                var result = MessageBox.Show("Update Success!!!", "Update", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    var mainPath = AppDomain.CurrentDomain.BaseDirectory;
                    var directoryInfo = new DirectoryInfo(mainPath);
                    var parentDirectory = directoryInfo.Parent;
                    var app = Path.Combine(parentDirectory.FullName, "LemonPlatform.Wpf.exe");

                    Process.Start(app);
                    Application.Current.Shutdown();
                }
            });
        }

        private void LogMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                logTextBox.AppendText(message + Environment.NewLine);
                logTextBox.ScrollToEnd();
            });
        }

        private void UpdateProgress(int processedEntries, int totalEntries)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                progressBar.Value = (double)processedEntries / totalEntries * 100;
            });
        }
    }
}