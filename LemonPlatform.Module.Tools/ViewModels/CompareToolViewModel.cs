using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LemonPlatform.Core.Infrastructures.Denpendency;
using System.Text;

namespace LemonPlatform.Module.Tools.ViewModels
{
    [ObservableObject]
    public partial class CompareToolViewModel: ITransientDependency
    {
        public CompareToolViewModel()
        {
            IsIgnoreCase = true;
        }

        [NotifyCanExecuteChangedFor(nameof(CompareCommand))]
        [ObservableProperty]
        private string _inputOne;

        [NotifyCanExecuteChangedFor(nameof(CompareCommand))]
        [ObservableProperty]
        private string _inputTwo;

        [ObservableProperty]
        private string _response;

        [ObservableProperty]
        private bool _isIgnoreCase;

        [RelayCommand(CanExecute = nameof(CompareCanExecute))]
        private void Compare()
        {
            var arr1 = InputOne.Trim().Split("\r\n").ToList();
            var arr2 = InputTwo.Trim().Split("\r\n").ToList();

            if (IsIgnoreCase)
            {
                for (var i = 0; i < arr1.Count; i++)
                {
                    arr1[i] = arr1[i].ToUpperInvariant().Trim();
                }

                for (var i = 0; i < arr2.Count; i++)
                {
                    arr2[i] = arr2[i].ToUpperInvariant().Trim();
                }
            }

            arr1 = arr1.Distinct().ToList();
            arr2 = arr2.Distinct().ToList();

            var intersect = arr1.Intersect(arr2);
            var union = arr1.Union(arr2);
            var exceptOne = arr1.Except(arr2);
            var exceptTwo = arr2.Except(arr1);

            var sb = new StringBuilder();
            sb.AppendLine("Intersect: ");
            foreach (var item in intersect)
            {
                sb.Append($"{item}\r\n");
            }

            sb.AppendLine("-------------------");

            sb.AppendLine("Only left have: ");
            foreach (var item in exceptOne)
            {
                sb.Append($"{item}\r\n");
            }

            sb.AppendLine("-------------------");

            sb.AppendLine("Only right have: ");
            foreach (var item in exceptTwo)
            {
                sb.Append($"{item}\r\n");
            }

            sb.AppendLine("-------------------");
            sb.AppendLine("Union: ");
            foreach (var item in union)
            {
                sb.Append($"{item}\r\n");
            }

            sb.AppendLine("-------------------");

            Response = sb.ToString();
        }

        private bool CompareCanExecute()
        {
            return !string.IsNullOrWhiteSpace(InputOne) && !string.IsNullOrWhiteSpace(InputTwo);
        }
    }
}