using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Graph.Views
{
    public partial class GraphView : Page, ISingletonDependency
    {
        public GraphView()
        {
            InitializeComponent();
        }
    }
}