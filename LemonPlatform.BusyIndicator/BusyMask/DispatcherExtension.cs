using System.Windows.Threading;

namespace LemonPlatform.BusyIndicator.BusyMask
{
    public static class DispatcherExtension
    {
        public static void Delay(this Dispatcher dispatcher, int delay, Action<object> action, object param = null)
        {
            Task.Delay(delay).ContinueWith((t) =>
            {
                dispatcher.Invoke(action, param);
            });
        }
    }
}