using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Wpf.Helpers;

namespace LemonPlatform.Wpf.ViewModels.UserControls
{
    [ObservableObject]
    public partial class FindNewVersionViewModel : ITransientDependency
    {
        public FindNewVersionViewModel()
        {
            var version = UpdateHelper.CheckForUpdatesAsync().Result;
        }

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private string _description;

        [RelayCommand]
        private void Ignore()
        {

        }

        [RelayCommand]
        private void Update()
        {

        }
    }
}