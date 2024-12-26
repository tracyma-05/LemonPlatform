using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;

namespace LemonPlatform.Wpf.ViewModels.Pages
{
    public partial class HomeViewModel : ObservableObject, ITransientDependency
    {
        [RelayCommand]
        private void Chat()
        {
            MessageHelper.SendMenuMessage("Chat");
        }

        [RelayCommand]
        private void Setting()
        {
            MessageHelper.SendMenuMessage("Setting");
        }
    }
}