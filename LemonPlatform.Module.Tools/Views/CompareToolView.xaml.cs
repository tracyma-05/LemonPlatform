using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Tools.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.Tools.Views
{
    public partial class CompareToolView : UserControl, ITransientDependency
    {
        public CompareToolView(CompareToolViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}