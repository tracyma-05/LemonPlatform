using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.CustomControls.Controls.TreeViews.Models;
using Microsoft.Win32;
using System.IO;

namespace LemonPlatform.Module.Tools.ViewModels
{
    [ObservableObject]
    public partial class RenameViewModel : ISingletonDependency
    {
        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [ObservableProperty]
        private string _filePath;

        [ObservableProperty]
        private string _response;

        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [ObservableProperty]
        private string _sourceInfo;

        [ObservableProperty]
        private string _destinationInfo;

        [RelayCommand]
        private void OpenDialog()
        {
            var dialog = new OpenFolderDialog
            {
                Multiselect = false,
            };

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                FilePath = dialog.FolderName;
                UpdateTreeView();
            }
        }

        [RelayCommand(CanExecute = nameof(CanRename))]
        private void Rename()
        {
            foreach (var item in Files)
            {
                if (item.Type == "Folder") continue;
                var fileName = item.Name.Replace(SourceInfo, DestinationInfo);
                var newPath = Path.Combine(Path.GetDirectoryName(item.Path), fileName + item.Extension);
                File.Move(item.Path, newPath);
            }

            UpdateTreeView();
        }

        [RelayCommand]
        private void Selection(FileItem item)
        {
            string name = item.Name;
        }

        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [ObservableProperty]
        private List<FileItem> _files;

        private void GetFiles(string root, List<FileItem> sources, int depth)
        {
            var dirs = Directory.GetDirectories(root);
            var files = Directory.GetFiles(root);
            foreach (var dir in dirs)
            {
                var item = new FileItem
                {
                    Name = Path.GetFileNameWithoutExtension(dir),
                    Path = dir,
                    Size = null,
                    Type = "Folder",
                    Depth = depth,
                    Children = new List<FileItem>(),
                };

                sources.Add(item);
                GetFiles(dir, item.Children, depth + 1);
            }

            foreach (var file in files)
            {
                var item = new FileItem
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Path = file,
                    Size = new FileInfo(file).Length,
                    Type = "File",
                    Depth = depth,
                    Extension = new FileInfo(file).Extension,
                };

                sources.Add(item);
            }
        }

        private bool CanRename()
        {
            return !string.IsNullOrEmpty(FilePath) && !string.IsNullOrWhiteSpace(SourceInfo) || Files != null && !Files.Any();
        }

        private void UpdateTreeView()
        {
            var source = new List<FileItem>();
            var depth = 0;
            GetFiles(FilePath, source, depth);

            Files = source;
        }
    }
}