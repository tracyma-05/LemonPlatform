using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class QuickThreeWaysSortView : UserControl, ISingletonDependency
    {
        public QuickThreeWaysSortView()
        {
            InitializeComponent();
        }
    }
}