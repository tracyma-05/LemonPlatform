using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Algorithm.Views
{
    public partial class ShellSortView : Page, ISingletonDependency
    {
        public ShellSortView()
        {
            InitializeComponent();
        }
    }
}