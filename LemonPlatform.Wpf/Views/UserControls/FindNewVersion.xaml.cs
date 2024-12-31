using LemonPlatform.Wpf.ViewModels.UserControls;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.UserControls
{
    public partial class FindNewVersion : UserControl 
    {
        public FindNewVersion(FindNewVersionViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}