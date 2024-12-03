using LemonPlatform.Core;
using LemonPlatform.Core.Enums;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.DataStructure.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LemonPlatform.Module.DataStructure
{
    public class DataStructureModule : ILemonModule
    {
        public List<PluginItem> GetMenuItems()
        {
            return new List<PluginItem>
            {
                new PluginItem("MaxHeap", typeof(MaxHeapView), "GoogleCirclesExtended", "#118B6D", "max heap data structure", PluginType.DataStructures),
                new PluginItem("BSTree", typeof(BSTreeView), "FamilyTree", "#17BA91", "binary search data structure", PluginType.DataStructures),
                new PluginItem("AVLTree", typeof(AVLTreeView), "FileTree", "#1DE9B6", "balanced binary data structure", PluginType.DataStructures),
                new PluginItem("RBTree", typeof(RBTreeView), "FileTreeOutline", "#0B5D48", "red black tree data structure", PluginType.DataStructures),
                new PluginItem("SkipList", typeof(SkipListView), "SkipNext", "#052E24", "skip list data structure", PluginType.DataStructures),
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