using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Json.DataStructures.SL;
using LemonPlatform.Module.Json.Models;
using SkiaSharp;

namespace LemonPlatform.Module.Json.Renders
{
    [ServiceRegister(typeof(ITreeRender), Lifetime.TransientDependency, nameof(SkipListRender))]
    public class SkipListRender : ITreeRender
    {
        public string Information { get; set; } =
            """
            跳表是可以实现二分查找的有序链表；

            每个元素插入时随机生成它的level；

            最底层包含所有的元素；

            如果一个元素出现在level(x)，那么它肯定出现在x以下的level中；

            每个索引节点包含两个指针，一个向下，一个向右；（笔记目前看过的各种跳表源码实现包括Redis 的zset 都没有向下的指针，那怎么从二级索引跳到一级索引呢？留个悬念，看源码吧，文末有跳表实现源码）

            跳表查询、插入、删除的时间复杂度为O(log n)，与平衡二叉树接近；



            为什么Redis选择使用跳表而不是红黑树来实现有序集合？
            Redis 中的有序集合(zset) 支持的操作：

            插入一个元素

            删除一个元素

            查找一个元素

            有序输出所有元素

            按照范围区间查找元素（比如查找值在 [100, 356] 之间的数据）

            其中，前四个操作红黑树也可以完成，且时间复杂度跟跳表是一样的。但是，按照区间来查找数据这个操作，红黑树的效率没有跳表高。按照区间查找数据时，跳表可以做到 O(logn) 的时间复杂度定位区间的起点，然后在原始链表中顺序往后遍历就可以了，非常高效。


            我们可以实现一个 randomLevel() 方法，该方法会随机生成 1~MAX_LEVEL 之间的数（MAX_LEVEL表示索引的最高层数），且该方法有 1/2 的概率返回 1、1/4 的概率返回 2、1/8的概率返回 3，以此类推。
            randomLevel() 方法返回 1 表示当前插入的该元素不需要建索引，只需要存储数据到原始链表即可（概率 1/2）
            randomLevel() 方法返回 2 表示当前插入的该元素需要建一级索引（概率 1/4）
            randomLevel() 方法返回 3 表示当前插入的该元素需要建二级索引（概率 1/8）
            randomLevel() 方法返回 4 表示当前插入的该元素需要建三级索引（概率 1/16）
            """;

        public ICollection<int> Keys { get; set; }

        private bool _inited;
        public bool Inited
        {
            get => _inited;
            set
            {
                if (_inited != value)
                {
                    _inited = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private int _height;
        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler RefreshRequested;

        private Random _random;
        private SkipList<int> _skipList;

        public void Add(int key)
        {
            _skipList.Add(key);
            Keys.Add(key);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Init()
        {
            if (!Inited)
            {
                _skipList = new SkipList<int>();
                if (Keys != null && Keys.Any())
                {
                    foreach (var item in Keys)
                    {
                        _skipList.Add(item);
                    }
                }
                else
                {
                    _random = new Random();
                    Keys = new List<int>();
                    for (int i = 0; i < 15; i++)
                    {
                        var val = _random.Next(0, 100);
                        _skipList.Add(val);
                        Keys.Add(val);
                    }
                }

                Inited = true;
            }
        }

        public void Remove(int key)
        {
            _skipList.Remove(key);
            Keys.Remove(key);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public bool Find(int key)
        {
            return _skipList.Find(key, out var result);
        }

        public void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            Init();
            GenerateTree();
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            DrawTree(canvas);
        }

        private List<List<SkipListPoint>> _points;
        private void GenerateTree()
        {
            Keys = new List<int>();

            var radius = 20;
            var xOffset = 50;
            var yOffset = 80;
            var yRaw = 50;

            var node = _skipList.Root;
            if (node == null) return;
            _points = new List<List<SkipListPoint>>();
            for (int i = 0; i < _skipList.Level; i++)
            {
                _points.Add(new List<SkipListPoint>());
            }

            var count = 1;
            while (node.Forwards[0] != null && node.Forwards[0] != null)
            {
                node = node.Forwards[0];
                var val = node.Value;
                var x = radius + count * xOffset;

                for (var i = node.Level - 1; i >= 0; i--)
                {
                    var currentPoint = new SkipListPoint
                    {
                        Key = val,
                        X = x,
                        Y = Height - yOffset * i - yRaw,
                    };

                    if (i > 0)
                    {
                        currentPoint.Down = new SkipListPoint
                        {
                            Key = val,
                            X = x,
                            Y = Height - yOffset * (i - 1) - yRaw,
                        };
                    }

                    _points[i].Add(currentPoint);
                }

                count++;
            }
        }

        private void DrawTree(SKCanvas canvas)
        {
            foreach (var point in _points)
            {
                if (point.Count == 0) continue;
                if (point.Count == 1)
                {
                    DrawCircle(canvas, point.First());
                }

                for (var i = 0; i < point.Count; i++)
                {
                    if (i == 0)
                    {
                        DrawCircle(canvas, point[0]);
                    }
                    else
                    {
                        var pre = point[i - 1];
                        var current = point[i];
                        DrawCircle(canvas, current);
                        DrawNextLine(canvas, pre, current, SKColors.Gray);
                    }

                    if (point[i].Down != null)
                    {
                        DrawNextLine(canvas, point[i], point[i].Down, SKColors.Gray, PathEffect.Dash);
                    }
                }
            }
        }

        private void DrawCircle(SKCanvas canvas, SkipListPoint node)
        {
            var radius = 20;
            using var circlePaint = new SKPaint
            {
                Color = SKColors.Blue,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                StrokeWidth = 2
            };

            canvas.DrawCircle(node.X, node.Y, radius, circlePaint);

            DrawNumber(canvas, node);
        }

        private void DrawNumber(SKCanvas canvas, SkipListPoint node)
        {
            using var textPaint = new SKPaint
            {
                Color = SKColors.White,
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

        private void DrawNextLine(SKCanvas canvas, SkipListPoint node1, SkipListPoint node2, SKColor color, PathEffect effect = PathEffect.Solid)
        {
            var radius = 20;
            var center1 = new SKPoint { X = node1.X, Y = node1.Y };
            var center2 = new SKPoint { X = node2.X, Y = node2.Y };

            using var linePaint = new SKPaint
            {
                Color = color,
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