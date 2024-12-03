using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class BSTreeView : UserControl, ISingletonDependency
    {
        public BSTreeView(BSTreeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}