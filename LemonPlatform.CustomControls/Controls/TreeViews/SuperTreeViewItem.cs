using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.CustomControls.Controls.TreeViews
{
    public class SuperTreeViewItem : TreeViewItem
    {
        static SuperTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuperTreeViewItem), new FrameworkPropertyMetadata(typeof(SuperTreeViewItem)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SuperTreeViewItem();
        }
    }
}