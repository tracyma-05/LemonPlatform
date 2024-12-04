using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Tools.Views
{
    public partial class CompareToolView : Page, ISingletonDependency
    {
        public CompareToolView()
        {
            InitializeComponent();
        }
    }
}