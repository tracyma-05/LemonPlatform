using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class SkipListView : Page, ITransientDependency
    {
        public SkipListView(SkipListViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (SkipListViewModel)DataContext;
            context.Input = string.Join(',', context.Render.Keys);
        }
    }
}