using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.UserControls
{
    public partial class ChatEmptyView : UserControl, ISingletonDependency
    {
        public ChatEmptyView()
        {
            InitializeComponent();
        }
    }
}