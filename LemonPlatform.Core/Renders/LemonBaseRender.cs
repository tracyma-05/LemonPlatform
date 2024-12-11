using LemonPlatform.Core.Extensions;
using LemonPlatform.Core.Models;
using SkiaSharp;

namespace LemonPlatform.Core.Renders
{
    public abstract class LemonBaseRender<T> : ILemonRender where T : new()
    {
        public abstract event EventHandler RefreshRequested;

        public Random Random { get; set; } = new Random();
        public T CoreData { get; set; } = new T();
        public virtual int InitCount { get; set; } = 15;
        public virtual int RangeMin { get; set; } = 0;
        public virtual int RangeMax { get; set; } = 100;
        public virtual bool IsAnimating { get; set; } = false;
        public virtual LemonSKPoint? AnimatingPoint { get; set; } = null;

        public virtual void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            InitRawData();
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            InitCanvasData(canvas, info);
            DrawInCanvas(canvas, info);

            if (IsAnimating && AnimatingPoint != null)
            {
                canvas.DrawLemonCircle(AnimatingPoint);
            }
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

        public abstract Task AddAsync(int key);
        public abstract Task RemoveAsync(int key);
        public abstract Task<bool> Contains(int key);
        public abstract bool IsEmpty();

        public abstract ICollection<int> Keys { get; set; }

        public abstract bool ReInit { get; set; }
        public abstract int Delay { get; set; }
        public abstract bool IsDebug { get; set; }
    }
}