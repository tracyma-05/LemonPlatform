using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Module.Hello.Views
{
    public partial class HelloView : UserControl, ITransientDependency
    {
        public HelloView()
        {
            InitializeComponent();
        }
    }
}