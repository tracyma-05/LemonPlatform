using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;

namespace LemonPlatform.Module.Hello.ViewModels
{
    [ObservableObject]
    public partial class HelloViewModel : ITransientDependency
    {
        public HelloViewModel()
        {
            
        }

        [ObservableProperty]
        private int _time;

        [RelayCommand]
        private async Task Dfs(CancellationToken token)
        {
            MessageHelper.SendBusyMessage(new Core.Models.BusyItem
            {
                IsBusy = true,
                Command = DfsCommand
            });

            await DfsAsync(token);
        }

        public async Task DfsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);

                Time += 1;
            }
        }
    }
}