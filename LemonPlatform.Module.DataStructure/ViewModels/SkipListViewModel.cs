using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.DataRenders;

namespace LemonPlatform.Module.DataStructure.ViewModels
{
    [ObservableObject]
    public partial class SkipListViewModel : ITransientDependency
    {
        public SkipListViewModel(SkipListRender render)
        {
            Render = render;
        }

        [ObservableProperty]
        private ILemonRender _render;
    }
}