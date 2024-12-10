using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Algorithm.Sorts.Basic;

namespace LemonPlatform.Module.Algorithm.Sorts
{
    public class SelectionSort : LemonSortBase, ITransientDependency
    {
        public override async Task RunAsync()
        {
            for (var i = 0; i < SortConfiguration.Count; i++)
            {
                var minIndex = i;
                await RenderAsync(i, minIndex, i);
                for (var j = i + 1; j < SortConfiguration.Count; j++)
                {
                    await RenderAsync(j, minIndex, i);
                    if (Numbers[j] < Numbers[minIndex])
                    {
                        minIndex = j;
                        await RenderAsync(j, minIndex, i);
                    }
                }

                Swap(i, minIndex);
                await RenderAsync(i, minIndex, i + 1);
            }

            await RenderAsync(-1, -1, -1, true);
        }

        private async Task RenderAsync(int currentIndex, int minIndex, int sortedIndex, bool completed = false)
        {
            RePainting(currentIndex, minIndex, sortedIndex, completed);
            await Delay();
        }

        private void RePainting(int currentIndex, int minIndex, int sortedIndex, bool completed = false)
        {
            foreach (var item in ObservablePoints)
            {
                if (item.X >= sortedIndex)
                {
                    item.PaintColor = SortConfiguration.UnSortColor;
                }

                if (item.X < sortedIndex)
                {
                    item.PaintColor = SortConfiguration.SortedColor;
                }

                if (item.X == currentIndex)
                {
                    item.PaintColor = SortConfiguration.CurrentColor;
                }

                if (item.X == minIndex)
                {
                    item.PaintColor = SortConfiguration.PivotColor;

                }

                if (completed)
                {
                    item.PaintColor = SortConfiguration.SortedColor;
                }

                item.Y = Numbers[item.MetaData.EntityIndex];
            }
        }
    }
}