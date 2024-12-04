using SkiaSharp;

namespace LemonPlatform.Core.Renders
{
    public interface ILemonRender
    {
        void PaintSurface(SKSurface surface, SKImageInfo info);
        event EventHandler RefreshRequested;
    }
}