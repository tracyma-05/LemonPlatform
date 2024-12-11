using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Puzzles.Models;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Game.ViewModels
{
    [ObservableObject]
    public partial class APuzzleADayViewModel : ITransientDependency
    {
        public APuzzleADayViewModel()
        {
            var names = EnumHelper.GetEnumNames<PuzzleType>();
            PuzzleTypes = new ObservableCollection<string>(names);
            SelectPuzzleTypeItem = PuzzleType.WithWeek.ToString();
        }

        [ObservableProperty]
        private ObservableCollection<string> _puzzleTypes;

        [ObservableProperty]
        private string _selectPuzzleTypeItem;

        [RelayCommand]
        private void Search()
        {

        }

        [RelayCommand]
        private void Pre()
        {

        }

        [RelayCommand]
        private void Next()
        {

        }
    }
}