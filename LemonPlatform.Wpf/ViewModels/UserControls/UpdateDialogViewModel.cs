using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Exceptions;
using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace LemonPlatform.Wpf.ViewModels.UserControls
{
    [ObservableObject]
    public partial class UpdateDialogViewModel
    {
        private readonly UpdateModel _updateModel;
        public UpdateDialogViewModel(UpdateModel updateModel)
        {
            _updateModel = updateModel;
            Status = LemonUpdateStatus.Init;
            InitUpdate();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RestartCommand))]
        [NotifyCanExecuteChangedFor(nameof(DownloadCommand))]
        private LemonUpdateStatus _status;

        [ObservableProperty]
        private long _total;

        [ObservableProperty]
        private long _current;

        [ObservableProperty]
        private string _logs;

        [ObservableProperty]
        private string _infos;

        [ObservableProperty]
        private string _currentFile;

        [RelayCommand(CanExecute = nameof(CanDownload))]
        private async Task Download(CancellationToken token)
        {
            Logs = $"Download start...{Environment.NewLine}";
            var dir = $"lemon-platform-{_updateModel.Version.Replace('.', '_')}";
            var tmpBasePath = Path.Combine(Path.GetTempPath(), dir);
            if (!Directory.Exists(tmpBasePath)) Directory.CreateDirectory(tmpBasePath);
            using var http = new HttpClient();
            var order = 0;
            while (_updateTasks.Count > 0)
            {
                var file = _updateTasks.Dequeue();
                Logs = $"Download file {file.FileName}{Environment.NewLine}";

                order++;
                CurrentFile = file.FileName;
                Total = file.FileSize;
                Current = 0;
                var tmpFilePath = Path.Combine(tmpBasePath, file.FileName);
                file.Source = tmpBasePath;

                if (File.Exists(tmpFilePath))
                {
                    var fileInfo = new FileInfo(tmpFilePath);
                    var fileSize = fileInfo.Length;
                    if (fileSize == file.FileSize)
                    {
                        _updates.Add(new ZipModel
                        {
                            Source = tmpFilePath,
                            Target = file.Target,
                            Order = order
                        });
                        continue;
                    }
                    else
                    {
                        File.Delete(tmpFilePath);
                    }
                }

                using (var response = await http.GetAsync(file.FileUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var buffer = new byte[8192];
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fs = new FileStream(tmpFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int read;
                            while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fs.WriteAsync(buffer, 0, read);
                                Current += read;
                            }
                        }
                    }
                }

                _updates.Add(new ZipModel
                {
                    Source = tmpFilePath,
                    Target = file.Target,
                    Order = order
                });


            }

            var content = JsonSerializer.Serialize(_updates);

            await File.WriteAllTextAsync("update.json", content, encoding: Encoding.UTF8);
            Status = LemonUpdateStatus.Downloaded;
        }

        private Queue<UpdateTask> _updateTasks = new Queue<UpdateTask>();
        private HashSet<ZipModel> _updates = new HashSet<ZipModel>();

        [RelayCommand(CanExecute = nameof(CanRestart))]
        private void Restart()
        {
            var updateFileName = "update.json";
            var updateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, updateFileName);
            var updater = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater\\LemonPlatform.Updater.exe");

            if (!File.Exists(updateFilePath)) throw new LemonException("can not find update.json.");
            if (!File.Exists(updater)) throw new LemonException("can not find update application.");

            Process.Start(updater, updateFilePath);
            Application.Current.Shutdown();
        }

        private bool CanRestart => Status == LemonUpdateStatus.Downloaded;

        private bool CanDownload => Status >= LemonUpdateStatus.Checked;

        #region private

        private void InitUpdate()
        {
            Infos = string.Empty;
            var isDesktopRuntimeInstalled = UpdateHelper.IsDotNetDesktopRuntimeInstalled("Microsoft.WindowsDesktop.App 9");
            Infos += $".NET 9 Windows Desktop Runtime: {isDesktopRuntimeInstalled}{Environment.NewLine}";
            Infos += $"{Environment.NewLine}";
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var moduleDir = Path.Combine(baseDir, "modules");
            if (_updateModel.Main.Any())
            {
                foreach (var item in _updateModel.Main)
                {
                    if (isDesktopRuntimeInstalled)
                    {
                        if (item.FileName.Contains("portal"))
                        {
                            Infos += $"Portal Version: {item.FileName} {HumanReadableFileSize(item.FileSize)}{Environment.NewLine}";
                            _updateTasks.Enqueue(new UpdateTask
                            {
                                FileName = item.FileName,
                                FileUrl = item.FileUrl,
                                FileSize = item.FileSize,
                                Target = baseDir,
                                IsMain = true
                            });
                        }
                    }
                    else
                    {
                        if (item.FileName.Contains("self-contained"))
                        {
                            Infos += $"Self Contained Version: {item.FileName} {HumanReadableFileSize(item.FileSize)}{Environment.NewLine}";

                            _updateTasks.Enqueue(new UpdateTask
                            {
                                FileName = item.FileName,
                                FileUrl = item.FileUrl,
                                FileSize = item.FileSize,
                                Target = baseDir,
                                IsMain = true
                            });
                        }
                    }
                }
            }

            Infos += $"{Environment.NewLine}";
            if (_updateModel.Modules.Any())
            {
                foreach (var item in _updateModel.Modules)
                {
                    Infos += $"Module: {item.FileName} {HumanReadableFileSize(item.FileSize)}{Environment.NewLine}";
                    var target = Path.Combine(moduleDir, item.FileName.Replace(".zip", ""));
                    _updateTasks.Enqueue(new UpdateTask
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        FileSize = item.FileSize,
                        Target = target,
                        IsMain = false
                    });
                }
            }

            Status = LemonUpdateStatus.Checked;
        }

        private string HumanReadableFileSize(double size)
        {
            var units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

        #endregion
    }
}