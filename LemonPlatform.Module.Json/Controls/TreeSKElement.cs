using LemonPlatform.Module.Json.Renders;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows;

namespace LemonPlatform.Module.Json.Controls
{
    public class TreeSKElement : SKElement
    {
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            TreeRender.PaintSurface(e.Surface, e.Info);
        }

        public ITreeRender TreeRender
        {
            get { return (ITreeRender)GetValue(TreeRenderProperty); }
            set { SetValue(TreeRenderProperty, value); }
        }

        public static readonly DependencyProperty TreeRenderProperty =
            DependencyProperty.Register("TreeRender", typeof(ITreeRender), typeof(TreeSKElement), new PropertyMetadata(new PropertyChangedCallback(RenderChanged)));

        private static TreeSKElement? sKRenderView;
        private static void RenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeSKElement)
            {
                sKRenderView = (TreeSKElement)d;
            }

            if (e.OldValue != null)
            {
                var oldValue = e.OldValue as ITreeRender;
                oldValue!.RefreshRequested -= Renderer_RefreshRequested;
            }

            if (e.NewValue != null)
            {
                var newValue = e.NewValue as ITreeRender;
                newValue!.RefreshRequested += Renderer_RefreshRequested;
            }
        }

        private static void Renderer_RefreshRequested(object? sender, EventArgs e)
        {
            sKRenderView?.InvalidateVisual();
        }
    }
}