using SkiaSharp;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LemonPlatform.Core.Helpers
{
    public class ScreenShotHelper
    {
        public static void ScreenShot(Control control)
        {
            var waterMark = $"Lemon Platform  Power By @Tracy Ma";
            var capturedImage = CaptureControlToSKBitmap(control);
            var watermarkedImage = AddWatermark(capturedImage, waterMark);

            BitmapSource bitmapSource = ConvertToBitmapSource(watermarkedImage);
            Clipboard.SetImage(bitmapSource);

            MessageHelper.SendSnackMessage("Image to Clipboard");
        }

        public static void SaveImage(SKBitmap bitmap, string filePath)
        {
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(filePath))
            {
                data.SaveTo(stream);
            }
        }

        public static SKBitmap AddWatermark(SKBitmap bitmap, string watermarkText)
        {
            using (var canvas = new SKCanvas(bitmap))
            {
                var paint = new SKPaint
                {
                    Color = SKColors.Red,
                    TextSize = 16,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    TextAlign = SKTextAlign.Right,
                    Typeface = SKTypeface.FromFamilyName("Cambria Math", SKFontStyle.Italic)
                };

                var textBounds = new SKRect();
                paint.MeasureText(watermarkText, ref textBounds);

                float x = bitmap.Width - 10;
                float y = bitmap.Height - 10;

                canvas.DrawText(watermarkText, x, y, paint);
            }

            return bitmap;
        }

        public static SKBitmap CaptureControlToSKBitmap(Control control)
        {
            var size = new Size(control.RenderSize.Width, control.RenderSize.Height);
            control.Measure(size);
            control.Arrange(new Rect(size));

            var renderTarget = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(control);

            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                using (var skImage = SKImage.FromEncodedData(stream))
                {
                    var skBitmap = SKBitmap.FromImage(skImage);

                    var resultBitmap = new SKBitmap(skBitmap.Width, skBitmap.Height);
                    using (var canvas = new SKCanvas(resultBitmap))
                    {
                        var color = ConvertBrushToSKColor(control.Background);
                        canvas.Clear(color);
                        canvas.DrawBitmap(skBitmap, 0, 0);
                    }

                    return resultBitmap;
                }
            }
        }

        public static BitmapSource ConvertToBitmapSource(SKBitmap skBitmap)
        {
            using (var skImage = SKImage.FromBitmap(skBitmap))
            using (var data = skImage.Encode(SKEncodedImageFormat.Png, 100))
            {
                using (var stream = new MemoryStream())
                {
                    data.SaveTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    return decoder.Frames[0];
                }
            }
        }

        private static SKColor ConvertBrushToSKColor(Brush brush)
        {
            if (brush is SolidColorBrush solidColorBrush)
            {
                Color mediaColor = solidColorBrush.Color;
                return new SKColor(mediaColor.R, mediaColor.G, mediaColor.B, mediaColor.A);
            }

            throw new InvalidOperationException("Only SolidColorBrush is supported for conversion.");
        }
    }
}