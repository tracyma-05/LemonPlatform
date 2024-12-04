using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Infrastructures.Ioc;
using LemonPlatform.Module.Json.Renders;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LemonPlatform.Module.Json.ViewModels
{
    [ObservableObject]
    public partial class JsonViewModel : ITransientDependency
    {

        public JsonViewModel()
        {
            InitDependencyData();
        }

        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;

        [ObservableProperty]
        private ITreeRender _render;

        [ObservableProperty]
        private int _key;

        [ObservableProperty]
        private string _input;

        [ObservableProperty]
        private string _response;

        [ObservableProperty]
        private ObservableCollection<string> _algorithms;

        [ObservableProperty]
        private string _selectAlgorithmItem;


        [RelayCommand]
        private void Add()
        {
            Response = $"Add Step:{Environment.NewLine}";
            Render.Add(Key);
            Input = string.Join(',', Render.Keys);
        }

        [RelayCommand]
        private void Remove()
        {
            Response = $"Remove Step:{Environment.NewLine}";
            Render.Remove(Key);
            Input = string.Join(',', Render.Keys);
        }

        [RelayCommand]
        private void Find()
        {
            Response = $"Find Step:{Environment.NewLine}";
            var result = Render.Find(Key);
            Response += $"Is {Key} exist: {result.ToString()}{Environment.NewLine}";
        }

        [RelayCommand]
        private void Load()
        {
            if (string.IsNullOrEmpty(Input)) return;
            var numbers = Input.Split(',').Select(int.Parse).ToList();
            Render.Keys = numbers;
            Render.Inited = false;
        }

        [RelayCommand]
        private async Task Info(CancellationToken token)
        {
            var info = Render.Information;
            if (string.IsNullOrEmpty(info)) return;

            //var view = new InfoView(info);
            //await DialogHost.Show(view, NextConstant.DialogHostIdentifier, null, null, null);
        }

        [RelayCommand]
        private void Save(UIElement control)
        {
            var waterMark = $"Data Structure: {SelectAlgorithmItem.Replace("Render", "")}     Power By: Tracy Ma";
            var capturedImage = CaptureControlToSKBitmap(control);
            var watermarkedImage = AddWatermark(capturedImage, waterMark);

            var filePath = Path.GetTempFileName().Replace(".tmp", ".png");
            SaveImage(watermarkedImage, filePath);

            BitmapSource bitmapSource = ConvertToBitmapSource(watermarkedImage);
            Clipboard.SetImage(bitmapSource);

            Response = filePath;
        }

        partial void OnSelectAlgorithmItemChanged(string? oldValue, string newValue)
        {
            if (newValue != null && newValue != oldValue)
            {
                Render = GetKeyedService<ITreeRender>(SelectAlgorithmItem);
                Render.Width = Width;
                Render.Height = Height;
            }
        }

        partial void OnHeightChanged(int oldValue, int newValue)
        {
            if (Render != null)
            {
                Render.Height = newValue;
            }
        }

        partial void OnWidthChanged(int oldValue, int newValue)
        {
            if (Render != null)
            {
                Render.Width = newValue;
            }
        }

        #region Private

        private void InitDependencyData()
        {
            Height = 625;
            Width = 941;

            var types = GetRenderTypes();
            Algorithms = new ObservableCollection<string>(types);
            SelectAlgorithmItem = types.First();

            Render = GetKeyedService<ITreeRender>(SelectAlgorithmItem);

            Render.Width = Width;
            Render.Height = Height;
        }

        private T GetKeyedService<T>(string name)
        {
            var service = IocManager.Instance.ServiceProvider.GetKeyedService<T>(name);
            //if (service == null) throw new NextException($"Can not find {name} service.");
            return service;
        }

        private List<string> GetRenderTypes()
        {
            var result = new List<string>();
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(ITreeRender).GetTypeInfo().IsAssignableFrom(x)
                          && x.GetTypeInfo().IsClass
                          && !x.GetTypeInfo().IsAbstract);

            foreach (var item in types)
            {
                result.Add(item.Name);
            }

            return result;
        }

        private SKBitmap CaptureControlToSKBitmap(UIElement control)
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
                        canvas.Clear(SKColors.White);
                        canvas.DrawBitmap(skBitmap, 0, 0);
                    }

                    return resultBitmap;
                }
            }
        }

        private SKBitmap AddWatermark(SKBitmap bitmap, string watermarkText)
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

        public void SaveImage(SKBitmap bitmap, string filePath)
        {
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = File.OpenWrite(filePath))
            {
                data.SaveTo(stream);
            }
        }

        private BitmapSource ConvertToBitmapSource(SKBitmap skBitmap)
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

        //public void Receive(NextMessage message)
        //{
        //    if (message.MessageType == Core.Enums.MessageTypeEnum.TreeStatus)
        //    {
        //        Response += $"{message.Content.ToString()}{Environment.NewLine}";
        //    }
        //}

        #endregion
    }
}