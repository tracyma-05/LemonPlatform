using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Core.Services;
using System.Collections.ObjectModel;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class ScheduleViewModel : ObservableObject, ITransientDependency
    {
        private readonly JobService _jobService;
        public ScheduleViewModel(JobService jobService)
        {
            _jobService = jobService;

            GetJobsAsync();
        }

        [ObservableProperty]
        private ObservableCollection<JobDetailDto> _jobs;

        private async Task GetJobsAsync()
        {
            var jobs = await _jobService.GetAllJobDetailsAsync();
            Jobs = new ObservableCollection<JobDetailDto>(jobs);
        }

        [RelayCommand]
        private async Task Pause(JobDetailDto options, CancellationToken token)
        {
            await _jobService.PauseJobAsync(options.JobKey, token);
            await GetJobsAsync();
        }

        [RelayCommand]
        private async Task Remove(JobDetailDto options, CancellationToken token)
        {
            await _jobService.DeleteJobAsync(options.JobKey, token);
            await GetJobsAsync();
        }

        [RelayCommand]
        private async Task Resume(JobDetailDto options, CancellationToken token)
        {
            await _jobService.ResumeJobAsync(options.JobKey, token);
            await GetJobsAsync();
        }
    }
}