using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private int _delay;

        [ObservableProperty]
        private int _key;

        [ObservableProperty]
        private string _input;

        [ObservableProperty]
        private string _response;

        [RelayCommand]
        private void Add()
        {
            Response = $"Add Step:{Environment.NewLine}";
            Render.Add(Key);
            Input = string.Join(',', Render.Keys);
        }

        [RelayCommand]
        private void Remove()
        {
            Response = $"Remove Step:{Environment.NewLine}";
            Render.Remove(Key);
            Input = string.Join(',', Render.Keys);
        }

        [RelayCommand]
        private void Find()
        {
            Response = $"Find Step:{Environment.NewLine}";
            var result = Render.Contains(Key);
            Response += $"Is {Key} exist: {result.ToString()}{Environment.NewLine}";
        }

        [RelayCommand]
        private void Load()
        {
            if (string.IsNullOrEmpty(Input)) return;
            var numbers = Input.Split(',').Select(int.Parse).ToList();
            Render.Keys = numbers;
            Render.ReInit = true;
        }
    }
}