using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Wpf.Models;

namespace LemonPlatform.Wpf.ViewModels.UserControls
{
    [ObservableObject]
    public partial class UpdateDialogViewModel
    {
        public UpdateDialogViewModel()
        {
            
        }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RestartCommand))]
        private LemonUpdateStatus _status;

        [ObservableProperty]
        private long _total;

        [ObservableProperty]
        private long _current;

        [RelayCommand(CanExecute = nameof(CanRestart))]
        private void Restart()
        {
        }

        private bool CanRestart => Status == LemonUpdateStatus.Success;
    }
}