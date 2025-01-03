using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Wpf.Helpers;
using LemonPlatform.Wpf.Models;
using System.IO;
using System.Net.Http;
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
            while (_updateTasks.Count > 0)
            {
                var file = _updateTasks.Dequeue();
                Logs = $"Download file {file.FileName}{Environment.NewLine}";

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
                        _updates.Add(file);
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

                _updates.Add(file);
            }

            Status = LemonUpdateStatus.Downloaded;
        }

        private Queue<UpdateTask> _updateTasks = new Queue<UpdateTask>();
        private HashSet<UpdateTask> _updates = new HashSet<UpdateTask>();

        [RelayCommand(CanExecute = nameof(CanRestart))]
        private void Restart()
        {
            if (_updateTasks == null || _updateTasks.Count > 0) return;
            while (_updateTasks.Count > 0)
            {
                var task = _updateTasks.Dequeue();
                if (string.IsNullOrWhiteSpace(task.Target)) continue;
                var dir = Path.GetDirectoryName(task.Target);

                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            }

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

                    _updateTasks.Enqueue(new UpdateTask
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        FileSize = item.FileSize,
                        Target = moduleDir,
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