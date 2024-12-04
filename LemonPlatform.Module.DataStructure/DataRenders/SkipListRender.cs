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
        public override ICollection<int> Keys { get; set; } = new List<int>();

        public override void InitRawData()
        {
            if (Keys.Any())
            {
                foreach (var item in Keys)
                {
                    CoreData.Add(item);
                }
            }
            else
            {
                for (var i = RangeMin; i < RangeMax; i++)
                {
                    var random = Random.Next(0, 100);
                    CoreData.Add(random);
                    Keys.Add(random);
                }
            }
        }

        private List<List<LemonSKPoint>> _points;
        public override void InitCanvasData()
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
                        Y = Height - yOffset * i - yRaw,
                        LineColor = SKColors.Gray,
                        CircleColor = SKColors.Blue,
                        TextColor = SKColors.Red,
                    };

                    if (i > 0)
                    {
                        currentPoint.Down = new LemonSKPoint
                        {
                            Key = val,
                            X = x,
                            Y = Height - yOffset * (i - 1) - yRaw,
                            LineColor = SKColors.Gray,
                            CircleColor = SKColors.Blue,
                            TextColor = SKColors.Red,
                        };
                    }

                    _points[i].Add(currentPoint);
                }

                count++;
            }
        }

        public override void DrawInCanvas(SKCanvas canvas)
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
    }
}