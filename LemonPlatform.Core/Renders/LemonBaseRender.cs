using SkiaSharp;

namespace LemonPlatform.Core.Renders
{
    public abstract class LemonBaseRender<T> : ILemonRender where T : new()
    {
        public event EventHandler RefreshRequested;

        public Random Random { get; set; } = new Random();
        public T CoreData { get; set; } = new T();
        public virtual int InitCount { get; set; } = 15;
        public virtual int RangeMin { get; set; } = 0;
        public virtual int RangeMax { get; set; } = 100;

        public virtual void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            if (!Keys.Any())
            {
                InitRawData();
            }

            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            InitCanvasData(canvas, info);
            DrawInCanvas(canvas, info);
        }

        /// <summary>
        /// step 1: init raw data
        /// </summary>
        public abstract void InitRawData();

        /// <summary>
        /// step 2: init canvas data
        /// </summary>
        public abstract void InitCanvasData(SKCanvas canvas, SKImageInfo info);

        /// <summary>
        /// step 3: draw image in canvas
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void DrawInCanvas(SKCanvas canvas, SKImageInfo info);

        /// <summary>
        /// keys for data
        /// </summary>
        public abstract ICollection<int> Keys { get; set; }
    }
}