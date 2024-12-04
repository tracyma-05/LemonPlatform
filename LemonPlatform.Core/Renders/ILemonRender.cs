using LemonPlatform.Core.DataStructures;
using SkiaSharp;

namespace LemonPlatform.Core.Renders
{
    public interface ILemonRender : IDataStructure
    {
        void PaintSurface(SKSurface surface, SKImageInfo info);
        event EventHandler RefreshRequested;
        bool ReInit { get; set; }
    }
}