using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class QuickThreeWaysSortView : Page, ISingletonDependency
    {
        public QuickThreeWaysSortView()
        {
            InitializeComponent();
        }
    }
}