using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Services;
using System.Windows;

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

        [RelayCommand]
        private void ScreenCapture()
        {
            var screenCapture = CustomControls.Controls.ScreenCuts.ScreenCapture.GetInstance(true);
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

            Thread.Sleep(300);

            screenCapture.CaptureCompleted += ScreenCaptureSnapCompleted;
            screenCapture.CaptureCanceled += ScreenCaptureSnapCanceled;
            screenCapture.Capture();
        }

        private void ScreenCaptureSnapCanceled()
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void ScreenCaptureSnapCompleted(System.Windows.Media.Imaging.CroppedBitmap bitmap)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }
}