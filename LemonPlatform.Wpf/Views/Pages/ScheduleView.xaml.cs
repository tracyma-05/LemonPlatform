using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class ScheduleView : UserControl, ITransientDependency
    {
        public ScheduleView(ScheduleViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}