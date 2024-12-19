using Application = System.Windows.Application;
using FontFamily = System.Windows.Media.FontFamily;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public static class DrawingContextHelper
    {
        public static FontFamily FontFamily = (FontFamily)Application.Current.TryFindResource("SC.NormalFontFamily");
    }
}