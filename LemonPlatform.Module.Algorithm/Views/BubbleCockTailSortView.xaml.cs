using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class BubbleCockTailSortView : Page, ISingletonDependency
    {
        public BubbleCockTailSortView()
        {
            InitializeComponent();
        }
    }
}