using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Visualization.Views
{
    public partial class SnowFlakeView : UserControl, ISingletonDependency
    {
        public SnowFlakeView()
        {
            InitializeComponent();
        }
    }
}