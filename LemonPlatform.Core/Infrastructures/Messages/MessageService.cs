using LemonPlatform.Core.Infrastructures.MainWindowService;
using System.Windows;

namespace LemonPlatform.Core.Infrastructures.Messages
{
    public class MessageService : IMessageService
    {
        public void ShowSnackMessage(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (Application.Current.MainWindow is IMainHostWindow window)
                {
                    window.AddSnackMessage(message);
                }
            });
        }
    }
}