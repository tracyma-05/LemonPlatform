using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.CustomControls.Controls.TreeViews.Models;
using Microsoft.Win32;
using System.IO;

namespace LemonPlatform.Module.Tools.ViewModels
{
    [ObservableObject]
    public partial class RenameViewModel : ISingletonDependency
    {
        public RenameViewModel()
        {
            Filter = ".*";
        }

        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [ObservableProperty]
        private string _sourcePath;

        [NotifyCanExecuteChangedFor(nameof(MoveCommand))]
        [ObservableProperty]
        private string _destinationPath;

        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [ObservableProperty]
        private string _sourceText;

        [ObservableProperty]
        private string _destinationText;

        [ObservableProperty]
        private string _filter;

        [RelayCommand]
        private void SelectSourcePath()
        {
            var dialog = new OpenFolderDialog
            {
                Multiselect = false,
            };

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                SourcePath = dialog.FolderName;
                UpdateTreeView();
            }
        }

        [RelayCommand]
        private void SelectDestinationPath()
        {
            var dialog = new OpenFolderDialog
            {
                Multiselect = false,
            };

            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                DestinationPath = dialog.FolderName;
            }
        }

        [RelayCommand(CanExecute = nameof(CanRename))]
        private void Rename()
        {
            RenameInternal(Files);
            UpdateTreeView();
        }

        [RelayCommand(CanExecute = nameof(CanMove))]
        private void Move()
        {
            MoveInternal(Files);
            UpdateTreeView();
            PathHelper.OpenPath(DestinationPath);
        }

        private void RenameInternal(List<FileItem> files)
        {
            foreach (var item in files)
            {
                if (item.Type == "Folder")
                {
                    RenameInternal(item.Children);
                    continue;
                }
                ;

                if (string.IsNullOrWhiteSpace(Filter) || Filter == ".*" || item.Extension == Filter)
                {
                    var fileName = item.Name.Replace(SourceText, DestinationText);
                    var newPath = Path.Combine(Path.GetDirectoryName(item.Path), fileName + item.Extension);
                    File.Move(item.Path, newPath);
                }
            }
        }

        private void MoveInternal(List<FileItem> files)
        {
            foreach (var item in files)
            {
                if (item.Type == "Folder")
                {
                    MoveInternal(item.Children);
                    continue;
                };

                if (string.IsNullOrWhiteSpace(Filter) || Filter == ".*" || item.Extension == Filter)
                {
                    var newPath = Path.Combine(DestinationPath, item.Name + item.Extension);
                    File.Move(item.Path, newPath);
                }
            }
        }

        [RelayCommand]
        private void Selection(FileItem item)
        {
            string name = item.Name;
        }

        [NotifyCanExecuteChangedFor(nameof(RenameCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveCommand))]
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
            return !string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(SourceText) && Files != null && Files.Any();
        }

        private bool CanMove()
        {
            return !string.IsNullOrWhiteSpace(DestinationPath) && Files != null && Files.Any();
        }

        private void UpdateTreeView()
        {
            var source = new List<FileItem>();
            var depth = 0;
            GetFiles(SourcePath, source, depth);

            Files = source;
        }
    }
}