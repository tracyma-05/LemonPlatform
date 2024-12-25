using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class RBTreeView : Page, ISingletonDependency
    {
        public RBTreeView(RBTreeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (RBTreeViewModel)DataContext;
            context.Input = string.Join(',', context.Render.Keys);
        }
    }
}