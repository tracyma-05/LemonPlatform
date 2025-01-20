using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Tools.Views
{
    public partial class RenameView : Page, ISingletonDependency
    {
        public RenameView()
        {
            InitializeComponent();
        }
    }
}