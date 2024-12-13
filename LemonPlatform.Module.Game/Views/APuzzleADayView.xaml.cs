using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;
using System.Windows.Input;

namespace LemonPlatform.Module.Game.Views
{
    public partial class APuzzleADayView : Page, ITransientDependency
    {
        public APuzzleADayView()
        {
            InitializeComponent();

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
    }
}