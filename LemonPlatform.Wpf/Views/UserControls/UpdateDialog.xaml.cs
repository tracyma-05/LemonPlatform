using LemonPlatform.Wpf.ViewModels.UserControls;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.UserControls
{
    public partial class UpdateDialog : UserControl
    {
        public UpdateDialog(UpdateDialogViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}