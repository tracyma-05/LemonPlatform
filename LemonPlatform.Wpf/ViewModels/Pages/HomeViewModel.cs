using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Services;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class HomeViewModel : ObservableObject, ITransientDependency
    {
        private readonly JobService _jobService;
        public HomeViewModel(JobService jobService)
        {
            _jobService = jobService;
        }

        [RelayCommand]
        private async Task GetJobs(CancellationToken token)
        {
            var jobs = await _jobService.GetAllJobDetailsAsync();
        }
    }
}