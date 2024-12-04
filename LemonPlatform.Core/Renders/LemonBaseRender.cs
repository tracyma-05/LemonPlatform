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

        private int _width;
        public virtual int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private int _height;
        public virtual int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public virtual void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            if (!Keys.Any())
            {
                InitRawData();
            }

            InitCanvasData();

            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            DrawInCanvas(canvas);
        }

        /// <summary>
        /// step 1: init raw data
        /// </summary>
        public abstract void InitRawData();

        /// <summary>
        /// step 2: init canvas data
        /// </summary>
        public abstract void InitCanvasData();

        /// <summary>
        /// step 3: draw image in canvas
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void DrawInCanvas(SKCanvas canvas);

        /// <summary>
        /// keys for data
        /// </summary>
        public abstract ICollection<int> Keys { get; set; }
    }
}