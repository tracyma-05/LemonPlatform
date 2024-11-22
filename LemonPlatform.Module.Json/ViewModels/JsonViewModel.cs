using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flurl.Http;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace LemonPlatform.Module.Json.ViewModels
{
    [ObservableObject]
    public partial class JsonViewModel : ITransientDependency
    {
        public JsonViewModel()
        {
        }

        [RelayCommand]
        private async void Set()
        {
            var url = "https://cn.bing.com/";
            Name = await url.GetStringAsync();
        }

        [ObservableProperty]
        private string _name;

        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                }
            };
    }
}