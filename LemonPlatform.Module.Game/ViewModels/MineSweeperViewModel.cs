using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Extensions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Data;
using SkiaSharp;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LemonPlatform.Module.Game.ViewModels
{
    [ObservableObject]
    public partial class MineSweeperViewModel : ITransientDependency
    {
        private MineSweeperData _mineSweeperData;
        private readonly int _blockSide = 32;
        public MineSweeperViewModel()
        {
            Count = 15;
            MineCount = 20;
            Delay = 5;

            GenerateMine();
        }

        [ObservableProperty]
        private ImageSource _imageSource;

        [ObservableProperty]
        private int _delay;

        [ObservableProperty]
        private int _mineCount;

        [ObservableProperty]
        private int _count;

        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;

        partial void OnCountChanged(int oldValue, int newValue)
        {
            if (newValue <= 0) newValue = 15;
            if (oldValue != newValue && newValue > 0 && _mineSweeperData != null)
            {
                GenerateMine();
            }
        }

        partial void OnMineCountChanged(int oldValue, int newValue)
        {
            if (newValue <= 0) newValue = 20;
            if (oldValue != newValue && newValue > 0 && _mineSweeperData != null)
            {
                GenerateMine();
            }
        }

        [RelayCommand]
        private async void Open(MouseButtonEventArgs e)
        {
            var image = e.Source as System.Windows.Controls.Image;
            if (image == null) return;

            var w = image.ActualWidth / _mineSweeperData.Width;
            var h = image.ActualHeight / _mineSweeperData.Height;

            var x = e.GetPosition(image).X;
            var y = e.GetPosition(image).Y;

            var width = x / w;
            var height = y / h;

            await SetDataAsync((int)height, (int)width, e.ChangedButton == MouseButton.Left);
        }

        [RelayCommand]
        private void ReStart()
        {
            GenerateMine();
        }

        private async Task SetDataAsync(int x, int y, bool isLeftClicked)
        {
            if (_mineSweeperData.InArea(x, y))
            {
                if (isLeftClicked)
                {
                    _mineSweeperData.Open(x, y);
                }
                else
                {
                    _mineSweeperData.Flags[x, y] = !_mineSweeperData.Flags[x, y];
                }
            }

            Render();
            await Task.Delay(Delay);
        }

        private void GenerateMine()
        {
            Width = Height = _blockSide * Count;
            _mineSweeperData = new MineSweeperData(Count, Count, MineCount);
            Render();
        }

        private void Render()
        {
            var writeableBitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, null);
            int width = (int)writeableBitmap.Width,
                height = (int)writeableBitmap.Height;

            writeableBitmap.Lock();
            var imageInfo = new SKImageInfo
            {
                Width = width,
                Height = height,
                AlphaType = SKAlphaType.Opaque,
                ColorType = SKColorType.Bgra8888,
                ColorSpace = SKColorSpace.CreateSrgb()
            };

            using var surface = SKSurface.Create(imageInfo, writeableBitmap.BackBuffer, width * 4);
            var canvas = surface.Canvas;
            for (int i = 0; i < _mineSweeperData.Width; i++)
            {
                for (var j = 0; j < _mineSweeperData.Height; j++)
                {
                    var x = j * _blockSide;
                    var y = i * _blockSide;

                    if (_mineSweeperData.Opens[i, j])
                    {
                        if (_mineSweeperData.IsMine(i, j))
                        {
                            canvas.PutImage(x, y, MineSweeperData.MineImageUrl);
                        }
                        else
                        {
                            var number = _mineSweeperData.GetNumber(i, j);
                            canvas.PutImage( x, y, MineSweeperData.NumberImageUrl(number));
                        }
                    }
                    else
                    {
                        if (_mineSweeperData.Flags[i, j])
                        {
                            canvas.PutImage( x, y, MineSweeperData.FlagImageUrl);
                        }
                        else
                        {
                            canvas.PutImage( x, y, MineSweeperData.BlockImageUrl);
                        }
                    }
                }
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            writeableBitmap.Unlock();

            ImageSource = writeableBitmap;
        }
    }
}