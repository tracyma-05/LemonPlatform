using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Puzzles.Core;
using LemonPlatform.Module.Game.Puzzles.Core.Desks;
using LemonPlatform.Module.Game.Puzzles.Core.Figures;
using LemonPlatform.Module.Game.Puzzles.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace LemonPlatform.Module.Game.ViewModels
{
    [ObservableObject]
    public partial class APuzzleADayViewModel : ITransientDependency
    {
        private readonly int _deskSize = 8;
        private ulong? _desk;
        public APuzzleADayViewModel()
        {
            var names = EnumHelper.GetEnumNames<PuzzleType>();
            PuzzleTypes = [.. names];
            SelectPuzzleTypeItem = PuzzleType.WithWeek.ToString();
            SelectedDate = DateTime.Now;

            InitPuzzleItems();
            InitDesks();
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
            if (!string.IsNullOrEmpty(newValue))
            {
                InitPuzzleItems();
                InitDesks();
            };
        }

        [ObservableProperty]
        private ObservableCollection<PuzzleItem> _puzzleItems;

        [ObservableProperty]
        private ObservableCollection<Desk> _desks;

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
        private async Task SearchAll(CancellationToken token)
        {
            var models = await GetFigureModelAsync();
            UpdateDesk(models);
        }

        [RelayCommand]
        private async Task SearchOne(CancellationToken token)
        {
            var models = await GetFigureModelAsync(count: 1);
            UpdateDesk(models);
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

            MessageHelper.SendStatusBarTextMessage($"{SelectedDate?.ToString("yyyy-MM-dd")} total solutions is: {Results.Count}, current: {ResultIndex + 1}");
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

            MessageHelper.SendStatusBarTextMessage($"{SelectedDate?.ToString("yyyy-MM-dd")} total solutions is: {Results.Count}, current: {ResultIndex + 1}");
        }

        [RelayCommand]
        private void Rotate(int index)
        {
            var desk = Desks[index];
            var puzzles = new PuzzleItem[16];
            for (int i = 0; i < 16; i++)
            {
                puzzles[i] = new PuzzleItem
                {
                    Background = Brushes.Transparent
                };
            }

            var kindIndex = (desk.KindIndex + 1) % desk.AllKinds.Count();
            var rows = desk.AllKinds[kindIndex].FigurePoints;
            for (var i = 0; i < rows.Count(); i++)
            {
                var data = rows[i].ToString();
                if (data == "1")
                {
                    PuzzleConstants.ColorMapping.TryGetValue(desk.Index, out var color);
                    puzzles[i].Background = color;
                }
            }

            desk.DeskItems = [.. puzzles];
            desk.KindIndex = kindIndex;

            Desks[index] = desk;
        }

        public void Drop(Desk rawDesk, int row, int column)
        {
            if (string.IsNullOrEmpty(SelectPuzzleTypeItem) || !SelectedDate.HasValue) return;

            var type = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            _desk ??= DeskPlus.CreateDesk(SelectedDate.Value, type);
            var figure = rawDesk.AllKinds[rawDesk.KindIndex];
            var newDesk = DeskPlus.GetFigurePlacement(_desk.Value, figure, row, column);
            if (newDesk == null)
            {
                MessageHelper.SendSnackMessage("Can't place here, please change it.");
                return;
            }

            var models = new List<DeskModel>();
            var bin = newDesk.Value.ToString("b").PadLeft(64, '0');
            for (var i = 0; i < bin.Count(); i++)
            {
                var item = bin[i].ToString();
                if (item == "1")
                {
                    var itemRow = 7 - i / 8;
                    var iteColumn = 7 - i % 8;

                    PuzzleConstants.ColorMapping.TryGetValue(rawDesk.Index, out var color);
                    models.Add(new DeskModel
                    {
                        RowNumber = itemRow,
                        ColumnNumber = iteColumn,
                        Background = color!,
                    });
                }
            }

            UpdateDesk(models);
            _desk = newDesk;
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

        private async Task<List<DeskModel>?> GetFigureModelAsync(int count = -1, int resultIndex = 0)
        {
            if (!SelectedDate.HasValue) return null; ;
            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);

            var resultPlus = await PlacementFinderPlus.FindAllAsync(SelectedDate.Value, puzzleType, count);
            if (!resultPlus.Any()) return null;
            Results = resultPlus;
            ResultIndex = 0;
            var solution1 = Results[resultIndex];
            var models = new List<DeskModel>();
            var index = 0;
            foreach (var solution in solution1)
            {
                var bin = solution.ToString("b").PadLeft(64, '0');
                for (var i = 0; i < bin.Count(); i++)
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

        private void InitDesks()
        {
            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var desks = FigurePlus.GetBundles(puzzleType);
            var result = new List<Desk>();
            var colorIndex = 0;
            var itemIndex = 0;
            foreach (var item in desks)
            {
                var puzzles = new PuzzleItem[16];
                for (int i = 0; i < 16; i++)
                {
                    puzzles[i] = new PuzzleItem
                    {
                        Background = Brushes.Transparent
                    };
                }

                var desk = new Desk
                {
                    DeskItems = [.. puzzles],
                    Index = itemIndex++,
                    AllKinds = item.GetKinds().ToArray()
                };

                var rows = desk.AllKinds[0].FigurePoints;
                for (var i = 0; i < rows.Count(); i++)
                {
                    var data = rows[i].ToString();
                    if (data == "1")
                    {
                        PuzzleConstants.ColorMapping.TryGetValue(colorIndex, out var color);
                        puzzles[i].Background = color;
                    }
                }

                result.Add(desk);
                colorIndex++;
            }

            Desks = new ObservableCollection<Desk>(result);
        }

        private bool CanPreExecute()
        {
            return ResultIndex >= 1;
        }

        private bool CanNextExecute()
        {
            return Results != null && ResultIndex <= Results.Count - 1 && ResultIndex >= 0 && Results.Count >= 2;
        }

        #endregion
    }
}