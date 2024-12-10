using CommunityToolkit.Diagnostics;
using LemonPlatform.Core.Helpers;
using System.IO;
using System.Reflection;

namespace LemonPlatform.Module.Game.Data
{
    public class MineSweeperData
    {
        private static string _imageUrlBase = $"{AssemblyDirectory}\\Resources";
        public static string BlockImageUrl = $"{_imageUrlBase}\\block.png";
        public static string FlagImageUrl = $"{_imageUrlBase}\\flag.png";
        public static string MineImageUrl = $"{_imageUrlBase}\\mine.png";

        private readonly int _width;
        private readonly int _height;
        private readonly bool[,] _mines;
        private readonly Random _random;
        private readonly int[,] _numbers;
        public bool[,] Opens { get; set; }
        public bool[,] Flags { get; set; }

        public MineSweeperData(int width, int height, int mineNumber)
        {
            Guard.IsGreaterThan(width, 0);
            Guard.IsGreaterThan(height, 0);
            Guard.IsInRange(mineNumber, 0, width * height);

            _width = width;
            _height = height;
            _random = new Random();

            _mines = new bool[_width, _height];
            Flags = new bool[_width, _height];
            Opens = new bool[_width, _height];
            _numbers = new int[_width, _height];

            GenerateMines(mineNumber);
            CalculateNumbers();
        }

        public int Width { get => _width; }
        public int Height { get => _height; }

        public bool IsMine(int x, int y)
        {
            if (!InArea(x, y)) ThrowHelper.ThrowArgumentOutOfRangeException();
            return _mines[x, y];
        }
        public bool InArea(int x, int y) => x >= 0 && x < _width && y >= 0 && y < _height;

        public static string NumberImageUrl(int num)
        {
            Guard.IsInRange(num, 0, 8, name: "No such a number image!");
            var baseUrl = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseUrl, $"{_imageUrlBase}\\{num}.png");
            return path;
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path)!;
            }
        }

        private void GenerateMines(int mineNumber)
        {
            for (int i = 0; i < mineNumber; i++)
            {
                var x = i / Width;
                var y = i % Width;
                _mines[x, y] = true;
            }

            for (int i = Width * Height - 1; i >= 0; i--)
            {
                var iX = i / Width;
                var iY = i % Width;
                var randomNumber = _random.Next(i + 1);

                var randomX = randomNumber / Width;
                var randomY = randomNumber % Width;

                Swap(iX, iY, randomX, randomY);
            }
        }

        private void Swap(int x1, int y1, int x2, int y2)
        {
            var t = _mines[x1, y1];
            _mines[x1, y1] = _mines[x2, y2];
            _mines[x2, y2] = t;
        }

        public int GetNumber(int x, int y)
        {
            if (!InArea(x, y)) ThrowHelper.ThrowArgumentOutOfRangeException();
            return _numbers[x, y];
        }

        private void CalculateNumbers()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (_mines[i, j]) _numbers[i, j] = -1;
                    _numbers[i, j] = 0;
                    for (var m = i - 1; m <= i + 1; m++)
                    {
                        for (var n = j - 1; n <= j + 1; n++)
                        {
                            if (InArea(m, n) && IsMine(m, n))
                            {
                                _numbers[i, j]++;
                            }
                        }
                    }
                }
            }
        }

        public void Open(int x, int y)
        {
            if (!InArea(x, y)) ThrowHelper.ThrowArgumentException("Out of index in Open function!");
            Opens[x, y] = true;
            if (IsMine(x, y)) MessageHelper.SendSnackMessage("Game Over!");
            if (_numbers[x, y] > 0) return;
            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (InArea(i, j) && !Opens[i, j] && !_mines[i, j])
                    {
                        Open(i, j);
                    }
                }
            }
        }
    }
}