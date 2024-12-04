using LemonPlatform.Core.Renders;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows;

namespace LemonPlatform.Core.Controls
{
    public class LemonSKElement : SKElement
    {
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            Render?.PaintSurface(e.Surface, e.Info);
        }

        public ILemonRender Render
        {
            get { return (ILemonRender)GetValue(RenderProperty); }
            set { SetValue(RenderProperty, value); }
        }

        public static readonly DependencyProperty RenderProperty =
            DependencyProperty.Register("Render", typeof(ILemonRender), typeof(LemonSKElement), new PropertyMetadata(RenderChanged));

        private static LemonSKElement? _dataStructureSKElement;
        private static void RenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LemonSKElement)
            {
                _dataStructureSKElement = (LemonSKElement)d;
            }

            if (e.OldValue != null && e.OldValue is ILemonRender oldRender)
            {
                oldRender.RefreshRequested -= RenderRefreshRequest;
            }

            if (e.NewValue != null && e.NewValue is ILemonRender newRender)
            {
                newRender.RefreshRequested += RenderRefreshRequest;
            }
        }

        private static void RenderRefreshRequest(object? sender, EventArgs e)
        {
            _dataStructureSKElement?.InvalidateVisual();
        }
    }
}