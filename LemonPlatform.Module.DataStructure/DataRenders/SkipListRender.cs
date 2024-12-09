using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Extensions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.Models.SL;
using SkiaSharp;
using System.Windows.Threading;

namespace LemonPlatform.Module.DataStructure.DataRenders
{
    public class SkipListRender : LemonBaseRender<SkipList<int>>, ITransientDependency
    {
        public override event EventHandler RefreshRequested;
        public override ICollection<int> Keys { get; set; } = new HashSet<int>();
        private Dictionary<int, HashSet<int>> _path;
        private List<LemonSKPoint> _pathPoint = new List<LemonSKPoint>();

        private bool _reInit;
        public override bool ReInit
        {
            get => _reInit;
            set
            {
                if (_reInit == value) return;
                _reInit = value;
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private int _delay;
        public override int Delay
        {
            get => _delay;
            set
            {
                if (_delay == value) return;
                _delay = value;
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void Add(int key)
        {
            CoreData.Add(key);
            Keys.Add(key);

            _path = CoreData.Paths["Add"];
            RefreshRequested?.Invoke(this, EventArgs.Empty);

            StartBallAnimation();
        }

        public override void Remove(int key)
        {
            CoreData.Remove(key, out int deleted);
            Keys.Remove(key);

            _path = CoreData.Paths["Remove"];
            RefreshRequested?.Invoke(this, EventArgs.Empty);

            StartBallAnimation();
        }

        public override bool Contains(int key)
        {
            var result = CoreData.Find(key, out var data);
            _path = CoreData.Paths["Find"];

            RefreshRequested?.Invoke(this, EventArgs.Empty);

            StartBallAnimation();
            return result;
        }

        private void StartBallAnimation()
        {
            IsAnimating = true;
            int index = 0;
            var animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(Delay)
            };

            animationTimer.Tick += (s, e) =>
            {
                if (index < _pathPoint.Count)
                {
                    AnimatingPoint = _pathPoint[index];

                    index++;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    animationTimer.Stop();
                    IsAnimating = false;
                    AnimatingPoint = null;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            };

            animationTimer.Start();
        }

        private List<List<LemonSKPoint>> _points;

        public override void InitRawData()
        {
            if (ReInit && Keys.Any())
            {
                CoreData.Clear();
                foreach (var item in Keys)
                {
                    CoreData.Add(item);
                }

                ReInit = false;
            }
            else if (CoreData.IsEmpty())
            {
                for (var i = 0; i < InitCount; i++)
                {
                    var random = Random.Next(RangeMin, RangeMax);
                    CoreData.Add(random);
                    Keys.Add(random);
                }
            }
        }

        public override void InitCanvasData(SKCanvas canvas, SKImageInfo info)
        {
            var radius = 20;
            var xOffset = 50;
            var yOffset = 80;
            var yRaw = 50;
            _pathPoint = new List<LemonSKPoint>();

            var node = CoreData.Root;
            if (node == null) return;

            _points = new List<List<LemonSKPoint>>();
            for (int i = 0; i < CoreData.Level; i++)
            {
                _points.Add(new List<LemonSKPoint>());
            }

            var count = 1;
            while (node.Forwards[0] != null && node.Forwards[0] != null)
            {
                node = node.Forwards[0];
                var val = node.Value;
                var x = radius + count * xOffset;

                for (var i = node.Level - 1; i >= 0; i--)
                {
                    var currentPoint = new LemonSKPoint
                    {
                        Key = val,
                        X = x,
                        Y = info.Height - yOffset * i - yRaw,
                        LineColor = SKColors.Gray,
                        CircleColor = SKColors.Blue,
                        TextColor = SKColors.White,
                    };

                    if (i > 0)
                    {
                        currentPoint.Down = new LemonSKPoint
                        {
                            Key = val,
                            X = x,
                            Y = info.Height - yOffset * (i - 1) - yRaw,
                            LineColor = SKColors.Gray,
                            CircleColor = SKColors.Blue,
                            TextColor = SKColors.White,
                        };
                    }

                    _points[i].Add(currentPoint);

                    if (_path != null && _path.ContainsKey(i) && _path[i].Count > 0 && _path[i].TryGetValue(currentPoint.Key, out int value))
                    {
                        _pathPoint.Add(new LemonSKPoint
                        {
                            Key = val,
                            X = x,
                            Y = info.Height - yOffset * i - yRaw,
                            LineColor = SKColors.Red,
                            CircleColor = SKColors.Red,
                            TextColor = SKColors.White,
                        });
                    }
                }

                count++;
            }
        }

        public override void DrawInCanvas(SKCanvas canvas, SKImageInfo info)
        {
            foreach (var point in _points)
            {
                if (point.Count == 0) continue;
                if (point.Count == 1)
                {
                    canvas.DrawLemonCircle(point.First());
                }

                for (var i = 0; i < point.Count; i++)
                {
                    if (i == 0)
                    {
                        canvas.DrawLemonCircle(point[0]);
                    }
                    else
                    {
                        var pre = point[i - 1];
                        var current = point[i];
                        canvas.DrawLemonCircle(current);
                        canvas.DrawLemonLine(pre, current);
                    }

                    if (point[i].Down != null)
                    {
                        canvas.DrawLemonLine(point[i], point[i].Down, PathEffect.Dash);
                    }
                }
            }
        }

        public override bool IsEmpty()
        {
            return CoreData.IsEmpty();
        }
    }
}