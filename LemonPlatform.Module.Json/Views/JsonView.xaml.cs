using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Json.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.Json.Views
{
    public partial class JsonView : Page, ITransientDependency
    {
        public JsonView(JsonViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void TreeSKElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var context = (JsonViewModel)DataContext;
            context.Height = (int)e.NewSize.Height;
            context.Width = (int)e.NewSize.Width;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (JsonViewModel)DataContext;
            context.Input = string.Join(',', context.Render.Keys);
        }
    }
}