using SkiaSharp;

namespace LemonPlatform.Module.Algorithm.Models
{
    public class SortConfiguration
    {
        public int Delay { get; set; } = 10;

        public int Count { get; set; } = 100;

        public int RandomBound { get; set; } = 500;

        public SKColor SortedColor { get; set; } = SKColors.Green;

        public SKColor UnSortColor { get; set; } = SKColors.Gray;

        public SKColor CurrentColor { get; set; } = SKColors.Red;

        public SKColor PivotColor { get; set; } = SKColors.Blue;

        public SKColor Blue { get; set; } = SKColors.Blue;

        public SKColor Yellow { get; set; } = SKColors.Yellow;

        public SKColor Brown { get; set; } = SKColors.Brown;

        public SKColor Orange { get; set; } = SKColors.Orange;
    }
}