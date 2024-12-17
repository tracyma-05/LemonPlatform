using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace LemonPlatform.Module.Game.Puzzles.Models
{
    [ObservableObject]
    public partial class DeskModel
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }

        [ObservableProperty]
        public Brush _background;

        [ObservableProperty]
        public string? _content;
    }
}