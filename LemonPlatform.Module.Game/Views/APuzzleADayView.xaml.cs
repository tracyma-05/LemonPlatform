using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Puzzles.Controls;
using LemonPlatform.Module.Game.Puzzles.Models;
using LemonPlatform.Module.Game.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LemonPlatform.Module.Game.Views
{
    public partial class APuzzleADayView : Page, ITransientDependency
    {
        private readonly APuzzleADayViewModel _model;
        public APuzzleADayView(APuzzleADayViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            _model = model;

            desk.PreviewMouseWheel += (sender, e) =>
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = sender
                };

                desk.RaiseEvent(eventArg);
            };
        }


        private void ListBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void desk_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var listBox = sender as ListBox;
                var listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (listBoxItem == null)
                    return;

                var data = listBoxItem.DataContext;
                DragDrop.DoDragDrop(listBoxItem, data, DragDropEffects.Copy);
            }
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        private void PuzzlePanel_Drop(object sender, DragEventArgs e)
        {
            var item = e.Data.GetData(typeof(Desk)) as Desk;
            if (item == null) return;
            var puzzle = sender as PuzzlePanel;
            if (puzzle == null) return;
            var dropPosition = e.GetPosition(puzzle);

            var row = (int)(dropPosition.Y / (puzzle.ActualHeight / puzzle.Rows));
            var column = (int)(dropPosition.X / (puzzle.ActualWidth / puzzle.Columns));

            _model.Drop(item, row, column);
        }
    }
}