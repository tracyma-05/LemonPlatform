using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Renders;
using LemonPlatform.Module.DataStructure.DataRenders;
using LemonPlatform.Module.DataStructure.Messages;

namespace LemonPlatform.Module.DataStructure.ViewModels
{
    [ObservableObject]
    public partial class AVLTreeViewModel : ITransientDependency, IRecipient<RenderMessage>
    {
        public AVLTreeViewModel(AVLTreeRender render)
        {
            WeakReferenceMessenger.Default.Register(this);

            Delay = 1000;
            Render = render;
            Render.Delay = Delay;
            Render.IsDebug = IsDebug;
        }

        [ObservableProperty]
        private ILemonRender _render;

        [ObservableProperty]
        private int _delay;

        [ObservableProperty]
        private bool _isDebug;

        [ObservableProperty]
        private int _key;

        [ObservableProperty]
        private string _input;

        [ObservableProperty]
        private string _response;

        partial void OnIsDebugChanged(bool oldValue, bool newValue)
        {
            Render.IsDebug = IsDebug;
        }

        #region Command

        [RelayCommand]
        private async Task Add(CancellationToken token)
        {
            Response = $"Add Step:{Environment.NewLine}";
            await Render.AddAsync(Key);
            Render.Keys.Add(Key);
            UpdateInput();
        }

        [RelayCommand]
        private async Task Remove(CancellationToken token)
        {
            Response = $"Remove Step:{Environment.NewLine}";
            await Render.RemoveAsync(Key);
            Render.Keys.Remove(Key);
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
            Response = string.Empty;
            var numbers = Input.Split(',').Select(int.Parse).ToList();
            Render.Keys = numbers;
            Render.ReInit = true;
        }

        [RelayCommand]
        private async Task LL(CancellationToken token)
        {
            Input = "10";
            LoadCommand.Execute(null);

            IsDebug = true;
            Key = 9;
            await AddCommand.ExecuteAsync(Key);

            Key = 8;
            await AddCommand.ExecuteAsync(Key);
            Input = "10,9,8";
            IsDebug = false;
        }

        [RelayCommand]
        private async Task RR(CancellationToken token)
        {
            Input = "8";
            LoadCommand.Execute(null);

            IsDebug = true;
            Key = 9;
            await AddCommand.ExecuteAsync(Key);

            Key = 10;
            await AddCommand.ExecuteAsync(Key);
            Input = "8,9,10";
            IsDebug = false;
        }

        [RelayCommand]
        private async Task LR(CancellationToken token)
        {
            Input = "15";
            LoadCommand.Execute(null);

            IsDebug = true;
            Key = 9;
            await AddCommand.ExecuteAsync(Key);

            Key = 10;
            await AddCommand.ExecuteAsync(Key);
            Input = "15,9,10";
            IsDebug = false;
        }

        [RelayCommand]
        private async Task RL(CancellationToken token)
        {
            Input = "10";
            LoadCommand.Execute(null);

            IsDebug = true;
            Key = 15;
            await AddCommand.ExecuteAsync(Key);

            Key = 11;
            await AddCommand.ExecuteAsync(Key);
            Input = "10,15,11";
            IsDebug = false;
        }

        #endregion

        #region private

        private void UpdateInput()
        {
            if (Render.Keys.Any())
            {
                Input = string.Join(',', Render.Keys);
            }
        }

        public void Receive(RenderMessage message)
        {
            if (message.Type == RenderType.AVLTree)
            {
                Response += $"{message.Message}{Environment.NewLine}";
            }
        }

        #endregion
    }
}