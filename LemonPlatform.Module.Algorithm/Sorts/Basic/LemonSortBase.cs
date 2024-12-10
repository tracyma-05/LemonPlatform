using LemonPlatform.Module.Algorithm.Models;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Algorithm.Sorts.Basic
{
    public abstract class LemonSortBase : ILemonSort
    {
        protected ISeries[] Series { get; set; }
        protected SortConfiguration SortConfiguration { get; set; } = new SortConfiguration();
        protected Random Random { get; set; } = new Random();
        protected ObservableCollection<SortObservablePoint> ObservablePoints { get; set; }
        protected int[] Numbers { get; set; }
        protected int GetCount() => Numbers.Length;
        protected async Task Delay() => await Task.Delay(SortConfiguration.Delay);
        protected void Swap(int i, int j)
        {
            if (i < 0 || i >= Numbers.Length || j < 0 || j >= Numbers.Length)
            {
                throw new ArgumentException("invalid index to access sort data.");
            }

            var tmp = Numbers[i];
            Numbers[i] = Numbers[j];
            Numbers[j] = tmp;
        }

        protected virtual void InitChart()
        {
            var series = new ColumnSeries<SortObservablePoint>
            {
                Values = ObservablePoints
            };

            series.OnPointMeasured(point =>
            {
                if (point.Visual is null) return;
                point.Visual.Fill = new SolidColorPaint(point.Model!.PaintColor);
            });

            Series = [series];
        }

        protected virtual void InitData(GenerationDataType generationDataType = GenerationDataType.Random)
        {
            ObservablePoints = new ObservableCollection<SortObservablePoint>();
            Numbers = new int[SortConfiguration.Count];
            var lBound = 1;
            var rBound = SortConfiguration.RandomBound;
            if (generationDataType == GenerationDataType.Identical)
            {
                var avgNumber = (lBound + rBound) / 2;
                lBound = avgNumber;
                rBound = avgNumber;
            }

            for (int i = 0; i < SortConfiguration.Count; i++)
            {
                var val = Random.Next(rBound - lBound + 1) + lBound;
                Numbers[i] = val;

            }

            if (generationDataType == GenerationDataType.NearlyOrdered)
            {
                Array.Sort(Numbers);
                var swapTime = (int)(0.01 * SortConfiguration.Count) + 1;
                for (var i = 0; i < swapTime; i++)
                {
                    var a = Random.Next(0, SortConfiguration.Count);
                    var b = Random.Next(0, SortConfiguration.Count);
                    Swap(a, b);
                }
            }

            for (int i = 0; i < SortConfiguration.Count; i++)
            {
                ObservablePoints.Add(new SortObservablePoint
                {
                    X = i,
                    Y = Numbers[i],
                    PaintColor = SortConfiguration.UnSortColor
                });
            }
        }


        public virtual ISeries[] GenerateSeries(int count, int bound, int delay, GenerationDataType generationDataType = GenerationDataType.Random)
        {
            SortConfiguration.Count = count;
            SortConfiguration.RandomBound = bound;
            SortConfiguration.Delay = delay;

            InitData(generationDataType);
            InitChart();

            return Series;
        }

        public abstract Task RunAsync();
        public virtual void UpdateDelay(int delay)
        {
            SortConfiguration.Delay = delay;
        }
    }
}