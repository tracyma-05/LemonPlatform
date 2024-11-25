using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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