using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class SkipListView : Page, ITransientDependency
    {
        public SkipListView()
        {
            InitializeComponent();
        }
    }
}