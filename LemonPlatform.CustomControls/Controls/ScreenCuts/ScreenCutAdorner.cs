using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Cursor = System.Windows.Input.Cursor;
using Cursors = System.Windows.Input.Cursors;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace LemonPlatform.CustomControls.Controls.ScreenCuts
{
    public class ScreenCutAdorner : Adorner
    {
        private const double THUMB_SIZE = 15;
        private const double MINIMAL_SIZE = 20;

        private readonly Canvas _canvas;
        private readonly VisualCollection _visuals;

        private readonly Thumb lc;
        private readonly Thumb tl;
        private readonly Thumb tc;
        private readonly Thumb tr;
        private readonly Thumb rc;
        private readonly Thumb br;
        private readonly Thumb bc;
        private readonly Thumb bl;

        public ScreenCutAdorner(UIElement adornedElement) : base(adornedElement)
        {
            var canvas = FindParent(adornedElement) as Canvas;
            if (canvas == null) throw new Exception("can not find canvas.");
            _canvas = canvas;

            lc = GetResizeThumb(Cursors.SizeWE, HorizontalAlignment.Left, VerticalAlignment.Center);
            tl = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Left, VerticalAlignment.Top);
            tc = GetResizeThumb(Cursors.SizeNS, HorizontalAlignment.Center, VerticalAlignment.Top);
            tr = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Right, VerticalAlignment.Top);
            rc = GetResizeThumb(Cursors.SizeWE, HorizontalAlignment.Right, VerticalAlignment.Center);
            br = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            bc = GetResizeThumb(Cursors.SizeNS, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            bl = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Left, VerticalAlignment.Bottom);

            _visuals = new VisualCollection(this) { lc, tl, tc, tr, rc, br, bc, bl };
        }

        #region override

        protected override int VisualChildrenCount => _visuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            var offset = THUMB_SIZE / 2;
            var size = new Size(THUMB_SIZE, THUMB_SIZE);
            lc.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height / 2 - offset), size));
            tl.Arrange(new Rect(new Point(-offset, -offset), size));
            tc.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, -offset), size));
            tr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), size));
            rc.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height / 2 - offset), size));
            br.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), size));
            bc.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, AdornedElement.RenderSize.Height - offset), size));
            bl.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), size));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        #endregion

        #region private

        private UIElement? FindParent(UIElement element)
        {
            var obj = VisualTreeHelper.GetParent(element);
            return obj as UIElement;
        }

        private Thumb GetResizeThumb(Cursor cursor, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            var thumb = new Thumb
            {
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                HorizontalAlignment = horizontal,
                VerticalAlignment = vertical,
                Cursor = cursor,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.White))
                }
            };

            var maxWidth = double.IsNaN(_canvas.Width) ? _canvas.ActualWidth : _canvas.Width;
            var maxHeight = double.IsNaN(_canvas.Height) ? _canvas.ActualHeight : _canvas.Height;
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null) return;
                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        var topHeight = element.Height - e.HorizontalChange;
                        if (topHeight > MINIMAL_SIZE)
                        {
                            var topChange = Canvas.GetTop(element) + e.VerticalChange;
                            if (topHeight > 0 && topChange >= 0)
                            {
                                element.Height = topHeight;
                                Canvas.SetTop(element, topChange);
                            }
                        }

                        break;
                    case VerticalAlignment.Bottom:
                        var bottomHeight = element.Height + e.VerticalChange;
                        if (bottomHeight > MINIMAL_SIZE)
                        {
                            var top = Canvas.GetTop(element) + bottomHeight;
                            if (bottomHeight > 0 && top <= _canvas.ActualHeight)
                                element.Height = bottomHeight;
                        }

                        break;
                    default:
                        break;
                }

                switch (thumb.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        if (element.Width - e.HorizontalChange > MINIMAL_SIZE)
                        {
                            var newWidth = element.Width - e.HorizontalChange;
                            var left = Canvas.GetLeft(element);
                            if (newWidth > 0 && left + e.HorizontalChange >= 0)
                            {
                                element.Width = newWidth;
                                Canvas.SetLeft(element, left + e.HorizontalChange);
                            }
                        }

                        break;
                    case HorizontalAlignment.Right:
                        if (element.Width + e.HorizontalChange > MINIMAL_SIZE)
                        {
                            var newWidth = element.Width + e.HorizontalChange;
                            var left = Canvas.GetLeft(element) + newWidth;
                            if (newWidth > 0 && left <= _canvas.ActualWidth)
                                element.Width = newWidth;
                        }

                        break;
                }

                e.Handled = true;
            };

            return thumb;
        }

        private FrameworkElementFactory GetFactory(Brush brush)
        {
            var ellipse = new FrameworkElementFactory(typeof(Ellipse));
            ellipse.SetValue(Shape.FillProperty, brush);
            ellipse.SetValue(Shape.StrokeProperty, ControlsHelper.PrimaryNormalBrush);
            ellipse.SetValue(Shape.StrokeThicknessProperty, 2);

            return ellipse;
        }

        private void Resize(FrameworkElement frameworkElement)
        {
            if (double.IsNaN(frameworkElement.Width)) frameworkElement.Width = frameworkElement.RenderSize.Width;
            if (double.IsNaN(frameworkElement.Height)) frameworkElement.Height = frameworkElement.RenderSize.Height;
        }

        #endregion
    }
}