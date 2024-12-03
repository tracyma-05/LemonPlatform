using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class BubbleCockTailSortView : UserControl, ISingletonDependency
    {
        public BubbleCockTailSortView()
        {
            InitializeComponent();
        }
    }
}