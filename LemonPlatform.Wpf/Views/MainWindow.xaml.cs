using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.MainWindowService;
using LemonPlatform.Wpf.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LemonPlatform.Wpf.Views
{
    public partial class MainWindow : Window, ITransientDependency, IMainHostWindow
    {
        public MainWindow(MainWindowViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            xContent.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };

            xContent.RaiseEvent(eventArg);
        }

        public void AddSnackMessage(string message)
        {
            var queue = MainSnackbar?.MessageQueue;
            Task.Run(() => queue?.Enqueue(message));
        }
    }
}