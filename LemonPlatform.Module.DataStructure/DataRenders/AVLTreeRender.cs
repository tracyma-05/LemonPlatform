using LemonPlatform.Core.Extensions;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.Models.AVL;
using SkiaSharp;

namespace LemonPlatform.Module.DataStructure.DataRenders
{
    public class AVLTreeRender : LemonBaseRender<AVLTree<int, int>>, ITransientDependency
    {
        public override event EventHandler RefreshRequested;
        public override ICollection<int> Keys { get; set; } = new HashSet<int>();

        #region properties

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

        private bool _isDebug;
        public override bool IsDebug
        {
            get => _isDebug;
            set
            {
                if (_isDebug == value) return;
                _isDebug = value;
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        public override async void Add(int key)
        {
            Keys.Add(key);
            await CoreData.Add(key, 0, Delay, IsDebug, RefreshRequested);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        public override bool Contains(int key)
        {
            return CoreData.Contains(key);
        }

        public override void DrawInCanvas(SKCanvas canvas, SKImageInfo info)
        {
            var root = CoreData.GetRoot();
            var queue = new Queue<AVLNode<int, int>>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                var drawNode = new LemonSKPoint
                {
                    X = node.X,
                    Y = node.Y,
                    Key = node.Key,
                    Height = node.Height,
                    CircleColor = node.CircleSKColor,
                    LineColor = SKColors.Gray,
                    TextColor = SKColors.White,
                    HeightColor = SKColors.Blue
                };

                canvas.DrawLemonCircle(drawNode);
                canvas.DrawLemonHeight(drawNode);
                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                    var leftNode = new LemonSKPoint
                    {
                        X = node.Left.X,
                        Y = node.Left.Y,
                        Key = node.Left.Key,
                        Height = node.Left.Height,
                        CircleColor = node.Left.CircleSKColor,
                        LineColor = SKColors.Gray,
                        TextColor = SKColors.White,
                    };

                    canvas.DrawLemonLine(drawNode, leftNode);
                }

                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                    var rightNode = new LemonSKPoint
                    {
                        X = node.Right.X,
                        Y = node.Right.Y,
                        Key = node.Right.Key,
                        Height = node.Right.Height,
                        CircleColor = node.Right.CircleSKColor,
                        LineColor = SKColors.Gray,
                        TextColor = SKColors.White,
                    };

                    canvas.DrawLemonLine(drawNode, rightNode);
                }
            }
        }

        public override void InitCanvasData(SKCanvas canvas, SKImageInfo info)
        {
            Keys = new HashSet<int>();
            var height = 100;
            var root = CoreData.GetRoot();
            root.X = info.Width / 2;
            root.Y = 35;
            root.Level = 0;

            var queue = new Queue<AVLNode<int, int>>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                var level = node.Level;
                Keys.Add(node.Key);

                var maxNodesAtLevel = Math.Pow(2, level + 1);
                var nodeWidth = info.Width / maxNodesAtLevel;

                if (node.Left != null)
                {
                    node.Left.Level = level + 1;
                    node.Left.Y = (level + 1) * height;
                    node.Left.X = (int)(node.X - nodeWidth * 0.5);

                    queue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    node.Right.Level = level + 1;
                    node.Right.Y = (level + 1) * height;
                    node.Right.X = (int)(node.X + nodeWidth * 0.5);

                    queue.Enqueue(node.Right);
                }
            }
        }

        public override async void InitRawData()
        {
            if (ReInit && Keys.Any())
            {
                CoreData = new AVLTree<int, int>();
                foreach (var item in Keys)
                {
                    await CoreData.Add(item, 0, Delay, IsDebug, RefreshRequested);
                }

                ReInit = false;
            }
            else if (CoreData.IsEmpty)
            {
                for (var i = 0; i < InitCount; i++)
                {
                    var random = Random.Next(RangeMin, RangeMax);
                    await CoreData.Add(random, 0, Delay, IsDebug, RefreshRequested);
                    Keys.Add(random);
                }
            }
        }

        public override bool IsEmpty()
        {
            return CoreData.IsEmpty;
        }

        public override void Remove(int key)
        {
            Keys.Remove(key);
            CoreData.Remove(key);
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}