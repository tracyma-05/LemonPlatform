using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.DataRenders;

namespace LemonPlatform.Module.DataStructure.ViewModels
{
    [ObservableObject]
    public partial class AVLTreeViewModel : ITransientDependency
    {
        public AVLTreeViewModel(AVLTreeRender render)
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
        private void Add()
        {
            Response = $"Add Step:{Environment.NewLine}";
            Render.Add(Key);
            UpdateInput();
        }

        [RelayCommand]
        private void Remove()
        {
            Response = $"Remove Step:{Environment.NewLine}";
            Render.Remove(Key);
            UpdateInput();
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