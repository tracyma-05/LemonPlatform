using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class BSTreeView : Page, ISingletonDependency
    {
        public BSTreeView()
        {
            InitializeComponent();
        }
    }
}