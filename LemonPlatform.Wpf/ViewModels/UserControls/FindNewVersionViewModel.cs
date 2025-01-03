using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Wpf.Models;
using LemonPlatform.Wpf.Views.UserControls;
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;

namespace LemonPlatform.Wpf.ViewModels.UserControls
{
    [ObservableObject]
    public partial class FindNewVersionViewModel
    {
        private readonly UpdateModel _updateModel;
        public FindNewVersionViewModel(UpdateModel model)
        {
            _updateModel = model;

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

        [ObservableProperty]
        private bool _isPopupVisible;

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
            if (DialogHost.IsDialogOpen(LemonConstants.RootDialog))
            {
                DialogHost.Close(LemonConstants.RootDialog, DialogResult.OK);

                var updateModel = new UpdateDialogViewModel(_updateModel);
                var view = new UpdateDialog(updateModel);

                MessageHelper.SendDialog(view);
            }
        }
    }
}