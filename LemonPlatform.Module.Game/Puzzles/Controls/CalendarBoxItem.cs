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

        public bool IsCurrentMonth
        {
            get { return (bool)GetValue(IsCurrentMonthProperty); }
            set { SetValue(IsCurrentMonthProperty, value); }
        }

        public static readonly DependencyProperty IsCurrentMonthProperty =
            DependencyProperty.Register("IsCurrentMonth", typeof(bool), typeof(CalendarBoxItem), new PropertyMetadata(false));

        public string DateFormat { get; set; }

        public DateTime Date { get; set; }
    }
}