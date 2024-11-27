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
        private JobDetailDto? _selectJob;

        [ObservableProperty]
        private ObservableCollection<JobDetailDto> _jobs;

        private async Task GetJobsAsync()
        {
            var jobs = await _jobService.GetAllJobDetailsAsync();
            Jobs = new ObservableCollection<JobDetailDto>(jobs);
        }


        [RelayCommand()]
        private async void Pause(JobDetailDto options)
        {
            await _jobService.PauseJobAsync(options.JobKey);
            await GetJobsAsync();
        }

        [RelayCommand]
        private async void Remove(JobDetailDto options)
        {
            await _jobService.DeleteJobAsync(options.JobKey);
            await GetJobsAsync();
        }

        [RelayCommand]
        private async void Start(JobDetailDto options)
        {
            await _jobService.StartJobAsync(options.JobKey);
            await GetJobsAsync();
        }
    }
}