using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class AVLTreeView : UserControl, ISingletonDependency
    {
        public AVLTreeView(AVLTreeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}