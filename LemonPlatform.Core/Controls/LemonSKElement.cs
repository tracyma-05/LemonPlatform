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

        private static void RenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as LemonSKElement;
            if (e.NewValue != null)
            {
                var render = e.NewValue as ILemonRender;
                render.RefreshRequested += (s, e) => element?.InvalidateVisual();
            }
        }
    }
}