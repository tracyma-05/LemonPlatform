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
                new PluginItem("bubble sort", typeof(BubbleSortView), "ChartBubble", "#bce672", "bubble sort", PluginType.Algorithm),
                new PluginItem("bubble sort(v2)", typeof(BubbleCockTailSortView), "ChartBubble", "#c9dd22", "bubble sort of cock tail", PluginType.Algorithm),
                new PluginItem("selection sort", typeof(SelectionSortView), "SelectionSearch", "#bddd22", "selection sort", PluginType.Algorithm),
                new PluginItem("shell sort", typeof(ShellSortView), "Powershell", "#afdd22", "shell sort", PluginType.Algorithm),
                new PluginItem("insert sort(v1)", typeof(InsertSortView), "AutoDownload", "#a3d900", "insert sort basic version", PluginType.Algorithm),
                new PluginItem("insert sort(v2)", typeof(InsertV2SortView), "AutoDownload", "#9ed900", "insert sort v2", PluginType.Algorithm),
                new PluginItem("heap sort", typeof(HeapSortView), "GraphOutline", "#9ed048", "heap sort", PluginType.Algorithm),
                new PluginItem("merge sort(v1)", typeof(MergeNoRecursionSortView), "SourceMerge", "#96ce54", "merge sort not recursion", PluginType.Algorithm),
                new PluginItem("merge sort(v2)", typeof(MergeRecursionSortView), "SourceMerge", "#00bc12", "merge sort with recursion", PluginType.Algorithm),
                new PluginItem("quick sort(v1)", typeof(QuickSortView), "Flash", "#0eb83a", "quick sort basic version", PluginType.Algorithm),
                new PluginItem("quick sort(v2)", typeof(QuickRandomPivotSortView), "FlashAlert", "#0eb83a", "quick sort with random pivot", PluginType.Algorithm),
                new PluginItem("quick sort(v3)", typeof(QuickTwoWaysSortView), "FlashAlertOutline", "#0aa344", "quick sort with two ways", PluginType.Algorithm),
                new PluginItem("quick sort(v4)", typeof(QuickThreeWaysSortView), "FlashAuto", "#16a951", "quick sort with three ways", PluginType.Algorithm),
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