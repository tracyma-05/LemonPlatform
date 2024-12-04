using LemonPlatform.Core.Infrastructures.Denpendency;
using SkiaSharp;

namespace LemonPlatform.Module.Json.Renders
{
    public interface ITreeRender : ITransientDependency
    {
        void PaintSurface(SKSurface surface, SKImageInfo info);
        event EventHandler RefreshRequested;

        int Width { get; set; }
        int Height { get; set; }

        void Add(int key);

        void Remove(int key);

        bool Find(int key);

        string Information { get; set; }
        ICollection<int> Keys { get; set; }

        bool Inited { get; set; }
        void Init();
    }
}