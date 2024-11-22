using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.MainWindowService;
using LemonPlatform.Wpf.ViewModels;
using System.Windows;

namespace LemonPlatform.Wpf.Views
{
    public partial class MainWindow : Window, ITransientDependency, IMainHostWindow
    {
        public MainWindow(MainWindowViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        public void AddSnackMessage(string message)
        {
            var queue = MainSnackbar?.MessageQueue;
            Task.Run(() => queue?.Enqueue(message));
        }
    }
}