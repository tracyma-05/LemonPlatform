using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Commons;
using LemonPlatform.Wpf.Models;
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;

namespace LemonPlatform.Wpf.ViewModels.UserControls
{
    [ObservableObject]
    public partial class FindNewVersionViewModel
    {
        public FindNewVersionViewModel(UpdateModel model)
        {
            Version = model.Version;
            CurrentVersion = model.CurrentVersion;
            Description = model.Description;
        }

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private string _currentVersion;

        [ObservableProperty]
        private string _description;

        [RelayCommand]
        private void Ignore()
        {
            if (DialogHost.IsDialogOpen(LemonConstants.RootDialog))
            {
                DialogHost.Close(LemonConstants.RootDialog, DialogResult.Cancel);
            }
        }

        [RelayCommand]
        private void Update()
        {

        }
    }
}