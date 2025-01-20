using System.Windows;
using System.Windows.Controls.Primitives;

namespace LemonPlatform.CustomControls.Controls.TreeViews
{
    public class ChevronSwitch : ToggleButton
    {
        static ChevronSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChevronSwitch), new FrameworkPropertyMetadata(typeof(ChevronSwitch)));
        }
    }
}