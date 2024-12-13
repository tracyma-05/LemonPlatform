using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Core.Models;
using LemonPlatform.Module.Game.Puzzles.Core;
using LemonPlatform.Module.Game.Puzzles.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace LemonPlatform.Module.Game.ViewModels
{
    [ObservableObject]
    public partial class APuzzleADayViewModel : ITransientDependency
    {
        private readonly int _deskSize = 8;
        public APuzzleADayViewModel()
        {
            var names = EnumHelper.GetEnumNames<PuzzleType>();
            PuzzleTypes = [.. names];
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

        /// <summary>
        /// store the results
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextCommand))]
        private List<ulong[]>? _results;

        /// <summary>
        /// result index
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(PreCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextCommand))]
        private int _resultIndex;

        [RelayCommand]
        private async Task Search(CancellationToken token)
        {
            MessageHelper.SendBusyMessage(new BusyItem
            {
                IsBusy = true,
                Command = SearchCommand
            });

            var models = await GetFigureModelAsync();
            UpdateDesk(models);

            MessageHelper.SendBusyMessage(new BusyItem
            {
                IsBusy = false,
                Command = SearchCommand
            });
        }

        [RelayCommand(CanExecute = nameof(CanPreExecute))]
        private void Pre()
        {
            var models = GetDeskModels(--ResultIndex);
            UpdateDesk(models);
            if (ResultIndex <= 0)
            {
                ResultIndex = 0;
            }
        }

        [RelayCommand(CanExecute = nameof(CanNextExecute))]
        private void Next()
        {
            var models = GetDeskModels(++ResultIndex);
            UpdateDesk(models);
            if (Results != null && ResultIndex >= Results.Count)
            {
                ResultIndex = Results.Count;
            }
        }

        #region private

        private void InitPuzzleItems()
        {
            ResultIndex = 0;
            Results = null;
            var data = GetPuzzleItems();
            if (data == null) return;
            PuzzleItems = [.. data];
        }

        private PuzzleItem[]? GetPuzzleItems()
        {
            if (string.IsNullOrEmpty(SelectPuzzleTypeItem) || !SelectedDate.HasValue) return null;

            var type = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var data = type == PuzzleType.WithWeek ? PuzzleConstants.PuzzleWithWeekData : PuzzleConstants.PuzzleWithoutWeekData;
            var day = SelectedDate.Value.Day;
            var week = (int)SelectedDate.Value.DayOfWeek;
            var month = SelectedDate.Value.Month;
            var needMark = new[] { PuzzleConstants.MonthData[month - 1], day.ToString(), PuzzleConstants.WeekData[week] };

            var result = new PuzzleItem[64];
            var index = 0;
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
                    puzzle.Background = Brushes.LightGray;
                }

                result[index] = puzzle;
                index++;
            }

            return result;
        }

        private async Task<List<DeskModel>?> GetFigureModelAsync(int resultIndex = 0)
        {
            if (!SelectedDate.HasValue) return null; ;
            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);

            var resultPlus = await PlacementFinderPlus.FindAllAsync(SelectedDate.Value, puzzleType);
            if (!resultPlus.Any()) return null;
            Results = resultPlus;
            ResultIndex = 0;
            var solution1 = Results[resultIndex];
            var models = new List<DeskModel>();
            var index = 0;
            foreach (var solution in solution1)
            {
                var bin = solution.ToString("b").PadLeft(64, '0');
                for (int i = 0; i < bin.Count(); i++)
                {
                    var item = bin[i].ToString();
                    if (item == "1")
                    {
                        var row = 7 - i / 8;
                        var column = 7 - i % 8;

                        PuzzleConstants.ColorMapping.TryGetValue(index, out var color);
                        models.Add(new DeskModel
                        {
                            RowNumber = row,
                            ColumnNumber = column,
                            Background = color!,
                        });
                    }
                }

                index++;
            }

            return models;
        }

        private IEnumerable<DeskModel> GetDeskModels(int currentIndex)
        {
            var current = Results[currentIndex];
            var models = new List<DeskModel>();
            var index = 0;
            foreach (var solution in current)
            {
                var bin = solution.ToString("b").PadLeft(64, '0');
                for (int i = 0; i < bin.Count(); i++)
                {
                    var item = bin[i].ToString();
                    if (item == "1")
                    {
                        var row = 7 - i / 8;
                        var column = 7 - i % 8;

                        PuzzleConstants.ColorMapping.TryGetValue(index, out var color);
                        models.Add(new DeskModel
                        {
                            RowNumber = row,
                            ColumnNumber = column,
                            Background = color!,
                        });
                    }
                }

                index++;
            }

            return models;
        }

        private void UpdateDesk(IEnumerable<DeskModel>? models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var data = GetPuzzleItems();
            if (data == null) return;
            foreach (var item in models)
            {
                var modelIndex = item.ColumnNumber + item.RowNumber * _deskSize;
                data[modelIndex].Background = item.Background;
            }

            PuzzleItems = [.. data];
        }

        private bool CanPreExecute()
        {
            return ResultIndex >= 1;
        }

        private bool CanNextExecute()
        {
            return Results != null && ResultIndex <= Results.Count - 1 && ResultIndex >= 0;
        }

        #endregion
    }
}