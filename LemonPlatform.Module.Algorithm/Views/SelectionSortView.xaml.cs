using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class SelectionSortView : Page, ISingletonDependency
    {
        public SelectionSortView()
        {
            InitializeComponent();
        }
    }
}