using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.Models.AVL;
using LemonPlatform.Module.DataStructure.Models.RB;
using SkiaSharp;

namespace LemonPlatform.Module.DataStructure.DataRenders
{
    public class RBTreeRender : LemonBaseRender<RBTree<int, int>>, ITransientDependency
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

        public override Task AddAsync(int key)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> Contains(int key)
        {
            throw new NotImplementedException();
        }

        public override void DrawInCanvas(SKCanvas canvas, SKImageInfo info)
        {
            throw new NotImplementedException();
        }

        public override void InitCanvasData(SKCanvas canvas, SKImageInfo info)
        {
            throw new NotImplementedException();
        }

        public override void InitRawData()
        {
            if (ReInit && Keys.Any())
            {
                CoreData = new RBTree<int, int>();
                foreach (var item in Keys)
                {
                    //await CoreData.Add(item, 0, Delay, IsDebug, RefreshRequested);
                }

                ReInit = false;
            }
            else if (CoreData.IsEmpty)
            {
                for (var i = 0; i < InitCount; i++)
                {
                    var random = Random.Next(RangeMin, RangeMax);
                    //await CoreData.Add(random, 0, Delay, IsDebug, RefreshRequested);
                    Keys.Add(random);
                }
            }
        }

        public override bool IsEmpty()
        {
            return CoreData.IsEmpty;
        }

        public override Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}