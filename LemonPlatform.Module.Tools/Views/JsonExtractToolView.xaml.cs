using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Tools.ViewModels;
using System.Windows.Controls;

namespace LemonPlatform.Module.Tools.Views
{
    public partial class JsonExtractToolView : UserControl, ITransientDependency
    {
        public JsonExtractToolView(JsonExtractToolViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}