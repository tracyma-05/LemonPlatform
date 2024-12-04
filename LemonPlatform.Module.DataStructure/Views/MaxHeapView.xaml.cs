using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class MaxHeapView : Page, ISingletonDependency
    {
        public MaxHeapView()
        {
            InitializeComponent();
        }
    }
}