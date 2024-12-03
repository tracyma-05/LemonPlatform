using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class SkipListView : UserControl, ISingletonDependency
    {
        public SkipListView(SkipListViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}