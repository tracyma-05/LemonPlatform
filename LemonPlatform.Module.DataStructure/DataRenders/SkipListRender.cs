using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Extensions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.Models.SL;
using SkiaSharp;

namespace LemonPlatform.Module.DataStructure.DataRenders
{
    public class SkipListRender : LemonBaseRender<SkipList<int>>, ITransientDependency
    {
        public override event EventHandler RefreshRequested;
        public override ICollection<int> Keys { get; set; } = new HashSet<int>();

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

        public override void Add(int key)
        {
            CoreData.Add(key);
            Keys.Add(key);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public override void Remove(int key)
        {
            CoreData.Remove(key);
            Keys.Remove(key);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public override bool Contains(int key)
        {
            return CoreData.Find(key, out var result);
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