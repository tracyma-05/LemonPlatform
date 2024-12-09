using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.DataStructure.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.DataStructure.Views
{
    public partial class AVLTreeView : Page, ITransientDependency
    {
        public AVLTreeView(AVLTreeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (AVLTreeViewModel)DataContext;
            context.Input = string.Join(',', context.Render.Keys);
        }
    }
}