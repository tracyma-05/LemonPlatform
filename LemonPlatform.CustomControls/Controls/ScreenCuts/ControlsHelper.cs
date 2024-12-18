using System.Windows;
using Application = System.Windows.Application;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public class ControlsHelper : DependencyObject
    {
        public static Brush PrimaryNormalBrush = (Brush)Application.Current.TryFindResource("SC.PrimaryNormalColor");
    }
}