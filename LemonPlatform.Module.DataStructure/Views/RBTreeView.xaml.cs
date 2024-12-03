using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class RBTreeView : UserControl, ISingletonDependency
    {
        public RBTreeView()
        {
            InitializeComponent();
        }
    }
}