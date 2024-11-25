using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.ViewModels.Pages;
using System.Windows.Controls;

namespace LemonPlatform.Wpf.Views.Pages
{
    public partial class LogView : UserControl, ISingletonDependency
    {
        public LogView(LogViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            logTextBox.ScrollToEnd();
        }
    }
}