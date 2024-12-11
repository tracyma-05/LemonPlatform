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
            Delay = 1000;
            Render = render;
            Render.Delay = Delay;
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
        private async Task Add(CancellationToken token)
        {
            Response = $"Add Step:{Environment.NewLine}";
            await Render.AddAsync(Key);
            UpdateInput();
        }

        [RelayCommand]
        private async Task Remove(CancellationToken token)
        {
            Response = $"Remove Step:{Environment.NewLine}";
            await Render.RemoveAsync(Key);
            UpdateInput();
        }

        [RelayCommand]
        private async Task Find(CancellationToken token)
        {
            Response = $"Find Step:{Environment.NewLine}";
            var result = await Render.Contains(Key);
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

        #region private

        private void UpdateInput()
        {
            if (Render.Keys.Any())
            {
                Input = string.Join(',', Render.Keys);
            }
        }

        #endregion
    }
}