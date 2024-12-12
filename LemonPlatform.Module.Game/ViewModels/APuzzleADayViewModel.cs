using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Puzzles.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

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
            SelectedDate = DateTime.Now;

            InitPuzzleItems();
        }

        [ObservableProperty]
        private DateTime? _selectedDate;
        partial void OnSelectedDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            if (newValue.HasValue)
            {
                InitPuzzleItems();
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> _puzzleTypes;

        [ObservableProperty]
        private string _selectPuzzleTypeItem;

        partial void OnSelectPuzzleTypeItemChanged(string? oldValue, string newValue)
        {
            if (!string.IsNullOrEmpty(newValue)) InitPuzzleItems();
        }

        [ObservableProperty]
        private ObservableCollection<PuzzleItem> _puzzleItems;

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

        private void InitPuzzleItems()
        {
            if (string.IsNullOrEmpty(SelectPuzzleTypeItem) || !SelectedDate.HasValue) return;

            var type = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var data = type == PuzzleType.WithWeek ? PuzzleConstants.PuzzleWithWeekData : PuzzleConstants.PuzzleWithoutWeekData;
            var day = SelectedDate.Value.Day;
            var week = (int)SelectedDate.Value.DayOfWeek;
            var month = SelectedDate.Value.Month;
            var needMark = new[] { PuzzleConstants.MonthData[month - 1], day.ToString(), PuzzleConstants.WeekData[week - 1] };

            var result = new List<PuzzleItem>();
            foreach (var item in data)
            {
                var puzzle = new PuzzleItem
                {
                    Content = item,
                    Background = Brushes.Transparent
                };

                if (string.IsNullOrEmpty(item))
                {
                    puzzle.Background = Brushes.Gray;
                }

                if (needMark.Contains(item))
                {
                    puzzle.Background = Brushes.Green;
                }

                result.Add(puzzle);
            }

            PuzzleItems = new ObservableCollection<PuzzleItem>(result);
        }
    }
}