using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class HeapSortView : Page, ISingletonDependency
    {
        public HeapSortView()
        {
            InitializeComponent();
        }
    }
}