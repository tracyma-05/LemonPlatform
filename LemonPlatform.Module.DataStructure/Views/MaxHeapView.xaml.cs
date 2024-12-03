using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class MaxHeapView : UserControl, ISingletonDependency
    {
        public MaxHeapView(MaxHeapViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}