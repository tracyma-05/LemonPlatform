using CommunityToolkit.Mvvm.Input;

namespace LemonPlatform.Core.Models
{
    public class BusyItem
    {
        public bool IsBusy { get; set; }

        public IAsyncRelayCommand Command { get; set; }
    }
}