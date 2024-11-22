using CommunityToolkit.Mvvm.ComponentModel;
using LemonPlatform.Core.Infrastructures.Denpendency;

namespace LemonPlatform.Module.Hello.ViewModels
{
    [ObservableObject]
    public partial class HelloViewModel : ITransientDependency
    {
        public HelloViewModel()
        {
            Name = "Hello View";
        }

        [ObservableProperty]
        private string _name;
    }
}