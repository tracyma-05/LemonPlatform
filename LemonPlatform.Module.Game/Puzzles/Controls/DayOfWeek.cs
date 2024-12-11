using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.Game.Puzzles.Controls
{
    public class DayOfWeek : Label 
    {
        static DayOfWeek()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DayOfWeek), new FrameworkPropertyMetadata(typeof(DayOfWeek)));
        }
    }
}