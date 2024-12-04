using LemonPlatform.Core;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Algorithm.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Module.Algorithm
{
    public class AlgorithmModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("bubble sort", typeof(BubbleSortView), "SortBoolAscending", "#FAA570", "bubble sort", PluginType.Algorithm),
                new PluginItem("bubble sort(v2)", typeof(BubbleCockTailSortView), "SortBoolAscending", "#FAA570", "bubble sort of cock tail", PluginType.Algorithm),
                new PluginItem("selection sort", typeof(SelectionSortView), "SortBoolAscending", "#FAA570", "selection sort", PluginType.Algorithm),
                new PluginItem("shell sort", typeof(ShellSortView), "SortBoolAscending", "#FAA570", "shell sort", PluginType.Algorithm),
                new PluginItem("insert sort(v1)", typeof(InsertSortView), "SortBoolAscending", "#FAA570", "insert sort basic version", PluginType.Algorithm),
                new PluginItem("insert sort(v2)", typeof(InsertV2SortView), "SortBoolAscending", "#FAA570", "insert sort v2", PluginType.Algorithm),
                new PluginItem("heap sort", typeof(HeapSortView), "SortBoolAscending", "#FAA570", "heap sort", PluginType.Algorithm),
                new PluginItem("merge sort(v1)", typeof(MergeNoRecursionSortView), "SortBoolAscending", "#FAA570", "merge sort not recursion", PluginType.Algorithm),
                new PluginItem("merge sort(v2)", typeof(MergeRecursionSortView), "SortBoolAscending", "#FAA570", "merge sort with recursion", PluginType.Algorithm),
                new PluginItem("quick sort(v1)", typeof(QuickSortView), "SortBoolAscending", "#FAA570", "quick sort basic version", PluginType.Algorithm),
                new PluginItem("quick sort(v2)", typeof(QuickRandomPivotSortView), "SortBoolAscending", "#FAA570", "quick sort with random pivot", PluginType.Algorithm),
                new PluginItem("quick sort(v3)", typeof(QuickTwoWaysSortView), "SortBoolAscending", "#FAA570", "quick sort with two ways", PluginType.Algorithm),
                new PluginItem("quick sort(v4)", typeof(QuickThreeWaysSortView), "SortBoolAscending", "#FAA570", "quick sort with three ways", PluginType.Algorithm),
            };
        }

        public void PostInit(IServiceProvider serviceProvider)
        {

        }

        public void RegisterServices(IServiceCollection services)
        {

        }
    }
}