using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class HomeView : Page, ITransientDependency
    {
        public HomeView(HomeViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}