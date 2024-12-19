using System.Windows;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public class ControlsHelper : DependencyObject
    {
        public static Brush PrimaryNormalBrush = (Brush)Application.Current.TryFindResource("SC.PrimaryNormalColorBrush");

        public static double CalculateAngle(Point start, Point end)
        {
            var radian = Math.Atan2(end.Y - start.Y, end.X - start.X);
            var angle = (radian * (180 / Math.PI) + 360) % 360;
            return angle;
        }
    }
}