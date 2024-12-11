using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.Game.Puzzles.Controls
{
    public class CalendarBox : ListBox
    {
        static CalendarBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarBox), new FrameworkPropertyMetadata(typeof(CalendarBox)));
        }
    }
}