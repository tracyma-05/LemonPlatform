using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Helpers;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Game.Puzzles.Core;
using LemonPlatform.Module.Game.Puzzles.Core.Desks;
using LemonPlatform.Module.Game.Puzzles.Core.Figures;
using LemonPlatform.Module.Game.Puzzles.Helpers;
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
        private readonly HashSet<int> _placedIndex = new HashSet<int>();

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
                ClearAll();
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
                ClearAll();
                InitDesks();
            };
        }

        [ObservableProperty]
        private ObservableCollection<DeskModel> _puzzleItems;

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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchScheduleOneCommand))]
        [NotifyCanExecuteChangedFor(nameof(SearchScheduleAllCommand))]
        private bool _isSchedule;

        [RelayCommand]
        private async Task SearchRandomAll(CancellationToken token)
        {
            var models = await GetFigureModelAsync();
            UpdateDesk(models);
        }

        [RelayCommand]
        private async Task SearchRandomOne(CancellationToken token)
        {
            var models = await GetFigureModelAsync(count: 1);
            UpdateDesk(models);
        }

        [RelayCommand(CanExecute = nameof(CanScheduleExecute))]
        private async Task SearchScheduleAll(CancellationToken token)
        {
            var models = await GetScheduleFigureModelAsync();
            UpdateDesk(models, _desk);
        }

        [RelayCommand(CanExecute = nameof(CanScheduleExecute))]
        private async Task SearchScheduleOne(CancellationToken token)
        {
            var models = await GetScheduleFigureModelAsync(count: 1);
            UpdateDesk(models, _desk);
        }

        [RelayCommand]
        private void ClearAll()
        {
            _desk = null;
            _placedIndex.Clear();
            InitPuzzleItems();
        }

        [RelayCommand(CanExecute = nameof(CanPreExecute))]
        private void Pre()
        {
            var models = GetDeskModels(--ResultIndex);
            UpdateDesk(models);
            ResultIndex = Math.Max(ResultIndex, 0);

            SendStatusMessage();
        }

        [RelayCommand(CanExecute = nameof(CanNextExecute))]
        private void Next()
        {
            var models = GetDeskModels(++ResultIndex);
            UpdateDesk(models);
            ResultIndex = Math.Min(ResultIndex, Results?.Count ?? 0);

            SendStatusMessage();
        }

        [RelayCommand]
        private void Rotate(int index)
        {
            var desk = Desks[index];
            var puzzles = CreateDemoDeskModels();
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
            if (_desk == null && !_placedIndex.Any()) ClearAll();

            if (string.IsNullOrEmpty(SelectPuzzleTypeItem) || !SelectedDate.HasValue) return;
            if (_placedIndex.Contains(rawDesk.Index))
            {
                MessageHelper.SendSnackMessage("This item had put in desk, please change one to put.");
                return;
            }

            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            _desk ??= DeskPlus.CreateDesk(SelectedDate.Value, puzzleType);
            var figure = rawDesk.AllKinds[rawDesk.KindIndex];
            var newDesk = DeskPlus.GetFigurePlacement(_desk.Value, figure, row, column);
            if (newDesk == null)
            {
                MessageHelper.SendSnackMessage("Can't place here, please change it.");
                return;
            }

            var models = new List<DeskModel>();
            var bin = newDesk.Value.ToString("b").PadLeft(64, '0');
            var keyDic = puzzleType == PuzzleType.WithWeek ? PuzzleConstants.ItemWithWeek : PuzzleConstants.ItemWithOutWeek;
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
                        Content = keyDic.GetValueOrDefault($"{row}-{column}")
                    });
                }
            }

            _desk |= newDesk;
            _placedIndex.Add(rawDesk.Index);
            UpdateDesk(models, _desk);
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

        private DeskModel[]? GetPuzzleItems(ulong? desk = null)
        {
            if (string.IsNullOrEmpty(SelectPuzzleTypeItem) || !SelectedDate.HasValue) return null;

            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var markedPoints = DateHelper.GetDateMarkedPoints(SelectedDate.Value, puzzleType);
            desk ??= DeskPlus.CreateDesk(markedPoints, puzzleType);
            if (desk == null) return null;

            var deskBinary = desk.Value.ToString("b").PadLeft(64, '0');
            var result = new List<DeskModel>();
            var keyDic = puzzleType == PuzzleType.WithWeek ? PuzzleConstants.ItemWithWeek : PuzzleConstants.ItemWithOutWeek;

            for (int i = deskBinary.Count() - 1; i >= 0; i--)
            {
                var item = deskBinary[i];
                var row = 7 - i / 8;
                var column = 7 - i % 8;

                var currentPoint = new DeskPoint(row, column);
                result.Add(new DeskModel
                {
                    RowNumber = row,
                    ColumnNumber = column,
                    Background = InitDeskBackground(item.ToString(), currentPoint, markedPoints),
                    Content = keyDic.GetValueOrDefault($"{row}-{column}")
                });
            }

            return result.ToArray();
        }

        private Brush InitDeskBackground(string val, DeskPoint currentPoint, IEnumerable<DeskPoint> markedPoints)
        {
            if (val == "0") return Brushes.Transparent;

            return markedPoints.Any(item => currentPoint.Row == item.Row && currentPoint.Column == item.Column + 1)
                ? Brushes.LightGray
                : Brushes.Gray;
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

        private async Task<List<DeskModel>?> GetScheduleFigureModelAsync(int count = -1, int resultIndex = 0)
        {
            if (_desk == null) return null;

            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var bundles = new List<FigurePlus>();
            var colorMapping = new Dictionary<int, Brush>();
            var bundleIndex = 0;
            var colorIndex = 0;
            foreach (var item in FigurePlus.GetBundles(puzzleType))
            {
                if (!_placedIndex.Contains(bundleIndex))
                {
                    bundles.Add(item);
                    PuzzleConstants.ColorMapping.TryGetValue(bundleIndex, out var color);
                    colorMapping.Add(colorIndex, color);
                    colorIndex++;
                }

                bundleIndex++;
            }

            var resultPlus = await PlacementFinderPlus.FindScheduleAllAsync(_desk.Value, bundles.ToArray(), count);
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

                        colorMapping.TryGetValue(index, out var color);
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
            var puzzleType = (PuzzleType)Enum.Parse(typeof(PuzzleType), SelectPuzzleTypeItem);
            var current = Results[currentIndex];
            var models = new List<DeskModel>();
            var index = 0;
            var colorMapping = new Dictionary<int, Brush>();
            if (IsSchedule && _desk != null && _placedIndex.Any())
            {
                var bundleIndex = 0;
                var colorIndex = 0;
                foreach (var item in FigurePlus.GetBundles(puzzleType))
                {
                    if (!_placedIndex.Contains(bundleIndex))
                    {
                        PuzzleConstants.ColorMapping.TryGetValue(bundleIndex, out var color);
                        colorMapping.Add(colorIndex, color);
                        colorIndex++;
                    }

                    bundleIndex++;
                }
            }
            else
            {
                colorMapping = PuzzleConstants.ColorMapping;
            }

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

                        colorMapping.TryGetValue(index, out var color);
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

        private void UpdateDesk(IEnumerable<DeskModel>? models, ulong? desk = null)
        {
            if (models == null || !models.Any())
            {
                var message = "There is no solution.";
                MessageHelper.SendStatusBarTextMessage(message);
                MessageHelper.SendSnackMessage(message);
                return;
            }

            var puzzleItems = PuzzleItems.Select(x => x).ToArray();
            foreach (var item in models)
            {
                var modelIndex = item.ColumnNumber + item.RowNumber * _deskSize;
                puzzleItems[modelIndex].Background = item.Background;
            }

            PuzzleItems = [.. puzzleItems];
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
                var puzzles = new DeskModel[16];
                for (int i = 0; i < 16; i++)
                {
                    puzzles[i] = new DeskModel
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
            return Results != null && ResultIndex < Results.Count - 1 && ResultIndex >= 0 && Results.Count >= 2;
        }

        private bool CanScheduleExecute()
        {
            return IsSchedule && _desk != null && _placedIndex.Any();
        }

        private void SendStatusMessage()
        {
            MessageHelper.SendStatusBarTextMessage($"{SelectedDate?.ToString("yyyy-MM-dd")} total solutions: {Results?.Count}, current: {ResultIndex + 1}");
        }

        private DeskModel[] CreateDemoDeskModels()
        {
            return Enumerable.Range(0, 16).Select(_ => new DeskModel { Background = Brushes.Transparent }).ToArray();
        }

        #endregion
    }
}