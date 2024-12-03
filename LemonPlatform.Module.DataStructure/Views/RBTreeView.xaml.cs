using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class RBTreeView : UserControl, ISingletonDependency
    {
        public RBTreeView(RBTreeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}