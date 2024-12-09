using SkiaSharp;

namespace LemonPlatform.Core.Models
{
    public class LemonSKPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Key { get; set; }
        public int Level { get; set; }
        public int Height { get; set; }
        public SKColor CircleColor { get; set; }

        public SKColor LineColor { get; set; }

        public SKColor TextColor { get; set; }
        public SKColor HeightColor { get; set; }

        public LemonSKPoint? Left { get; set; }
        public LemonSKPoint? Right { get; set; }
        public LemonSKPoint? Down { get; set; }
        public LemonSKPoint? Up { get; set; }        
    }
}