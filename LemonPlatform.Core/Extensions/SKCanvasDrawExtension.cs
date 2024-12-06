using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using SkiaSharp;

namespace LemonPlatform.Core.Extensions
{
    public static class SKCanvasDrawExtension
    {
        public static void DrawLemonCircle(this SKCanvas canvas, LemonSKPoint node)
        {
            var radius = 20;
            using var circlePaint = new SKPaint
            {
                Color = node.CircleColor,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                StrokeWidth = 2
            };

            canvas.DrawCircle(node.X, node.Y, radius, circlePaint);
            DrawLemonNumber(canvas, node);
        }

        public static void DrawLemonNumber(this SKCanvas canvas, LemonSKPoint node)
        {
            using var textPaint = new SKPaint
            {
                Color = node.TextColor,
                IsAntialias = true,
                TextSize = 20,
                TextAlign = SKTextAlign.Center
            };

            var text = node.Key.ToString();
            var textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);
            var textX = (float)node.X;
            var textY = (float)(node.Y - textBounds.MidY);

            canvas.DrawText(text, textX, textY, textPaint);
        }

        public static void DrawLemonLine(this SKCanvas canvas, LemonSKPoint node1, LemonSKPoint node2, PathEffect effect = PathEffect.Solid)
        {
            var radius = 20;
            var center1 = new SKPoint { X = node1.X, Y = node1.Y };
            var center2 = new SKPoint { X = node2.X, Y = node2.Y };

            using var linePaint = new SKPaint
            {
                Color = node1.LineColor,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };

            if (effect == PathEffect.Dash)
            {
                linePaint.PathEffect = SKPathEffect.CreateDash([10, 5], 0);
            }

            var direction = new SKPoint(center2.X - center1.X, center2.Y - center1.Y);
            var length = SKPoint.Distance(center1, center2);

            var unitDirection = new SKPoint(direction.X / length, direction.Y / length);

            var start = new SKPoint(center1.X + unitDirection.X * radius, center1.Y + unitDirection.Y * radius);
            var end = new SKPoint(center2.X - unitDirection.X * radius, center2.Y - unitDirection.Y * radius);

            canvas.DrawLine(start, end, linePaint);
        }
    }
}