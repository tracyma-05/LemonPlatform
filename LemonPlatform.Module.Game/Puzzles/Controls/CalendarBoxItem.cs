using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.Game.Puzzles.Controls
{
    public class CalendarBoxItem : ListBoxItem
    {
        static CalendarBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarBoxItem), new FrameworkPropertyMetadata(typeof(CalendarBoxItem)));
        }
    }
}