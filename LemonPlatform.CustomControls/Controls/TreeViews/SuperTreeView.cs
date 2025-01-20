using LemonPlatform.CustomControls.Controls.TreeViews.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard;
using TreeView = System.Windows.Controls.TreeView;

namespace LemonPlatform.CustomControls.Controls.TreeViews
{
    public class SuperTreeView : TreeView
    {
        static SuperTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuperTreeView), new FrameworkPropertyMetadata(typeof(SuperTreeView)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SuperTreeViewItem();
        }

        public SuperTreeView()
        {
            SelectedItemChanged += OnSelectedItemChanged;
            MouseRightButtonDown += OnMouseRightButtonDown;
        }

        public static readonly DependencyProperty SelectionCommandProperty =
            DependencyProperty.Register(nameof(SelectionCommand), typeof(ICommand), typeof(SuperTreeView), new PropertyMetadata(null));

        public ICommand SelectionCommand
        {
            get { return (ICommand)GetValue(SelectionCommandProperty); }
            set { SetValue(SelectionCommandProperty, value); }
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            e.Handled = true;
            if (SelectionCommand != null && e.NewValue is FileItem item && SelectionCommand.CanExecute(item))
            {
                SelectionCommand.Execute(item);
            }
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItem is FileItem selectedItem)
            {
                ShowContextMenu(selectedItem);
            }
        }

        private void ShowContextMenu(FileItem selectedItem)
        {
            var contextMenu = new ContextMenu();
            var openMenuItem = GenerateOpenMenu(selectedItem);
            var fileNameMenuItem = GenerateFileNameMenu(selectedItem);

            contextMenu.Items.Add(openMenuItem);
            contextMenu.Items.Add(fileNameMenuItem);
            contextMenu.IsOpen = true;
        }

        private MenuItem GenerateOpenMenu(FileItem selectedItem)
        {
            var menuItem = new MenuItem { Header = "_Open" };
            menuItem.Click += (s, e) =>
            {
                Process.Start("explorer.exe", selectedItem.Path);
            };

            return menuItem;
        }

        private MenuItem GenerateFileNameMenu(FileItem selectedItem)
        {
            var menuItem = new MenuItem { Header = "_Copy Name" };
            menuItem.Click += (s,e)=>
            {
                Clipboard.SetText(selectedItem.Name);
            };
            return menuItem;
        }
    }
}