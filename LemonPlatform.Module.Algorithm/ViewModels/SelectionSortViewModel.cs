using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using LemonPlatform.Module.Algorithm.Models;
using LemonPlatform.Module.Algorithm.Sorts;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace LemonPlatform.Module.Algorithm.ViewModels
{
    [ObservableObject]
    public partial class SelectionSortViewModel : ITransientDependency
    {
        private readonly SelectionSort _selectionSort;
        public SelectionSortViewModel(SelectionSort selectionSort)
        {
            _selectionSort = selectionSort;

            var names = GetEnumNames<GenerationDataType>();
            DataTypes = new ObservableCollection<string>(names);
            SelectDataTypeItem = GenerationDataType.Random.ToString();

            Count = 100;
            Bound = 500;
            Delay = 20;

            Series = selectionSort.GenerateSeries(Count, Bound, Delay);
            Y = [new Axis { MinLimit = 0 }];
        }

        #region properties

        [ObservableProperty]
        public ISeries[] _series;

        [ObservableProperty]
        private int _count;

        [ObservableProperty]
        private int _bound;

        [ObservableProperty]
        private int _delay;

        [ObservableProperty]
        private Axis[] _y;

        [ObservableProperty]
        private ObservableCollection<string> _dataTypes;

        [ObservableProperty]
        private string _selectDataTypeItem;

        partial void OnCountChanged(int oldValue, int newValue)
        {
            ConfigurationChanged(oldValue, newValue);
        }

        partial void OnBoundChanged(int oldValue, int newValue)
        {
            ConfigurationChanged(oldValue, newValue);
        }

        partial void OnDelayChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && _selectionSort != null)
            {
                _selectionSort.UpdateDelay(newValue);
            }
        }

        partial void OnSelectDataTypeItemChanged(string? oldValue, string newValue)
        {
            if (string.CompareOrdinal(oldValue, newValue) != 0 && _selectionSort != null)
            {
                Series = _selectionSort.GenerateSeries(Count, Bound, Delay, Enum.Parse<GenerationDataType>(SelectDataTypeItem));
            }
        }

        #endregion

        [RelayCommand]
        private async Task Run(CancellationToken token)
        {
            await _selectionSort.RunAsync();
        }

        private string[] GetEnumNames<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T));
        }

        private void ConfigurationChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && _selectionSort != null)
            {
                Series = _selectionSort.GenerateSeries(Count, Bound, Delay, Enum.Parse<GenerationDataType>(SelectDataTypeItem));
            }
        }
    }
}