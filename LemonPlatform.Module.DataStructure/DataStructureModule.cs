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
                new PluginItem("MaxHeap", typeof(MaxHeapView), "GoogleCirclesExtended", "#70f3ff", "max heap data structure", PluginType.DataStructures),
                new PluginItem("BSTree", typeof(BSTreeView), "FamilyTree", "#44cef6", "binary search data structure", PluginType.DataStructures),
                new PluginItem("AVLTree", typeof(AVLTreeView), "FileTree", "#3eede7", "balanced binary data structure", PluginType.DataStructures),
                new PluginItem("RBTree", typeof(RBTreeView), "FileTreeOutline", "#1685a9", "red black tree data structure", PluginType.DataStructures),
                new PluginItem("SkipList", typeof(SkipListView), "SkipNext", "#177cb0", "skip list data structure", PluginType.DataStructures),
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