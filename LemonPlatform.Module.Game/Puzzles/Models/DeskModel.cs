using System.Windows.Media;

namespace LemonPlatform.Module.Game.Puzzles.Models
{
    public class DeskModel
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public Brush Background { get; set; } = Brushes.Transparent;
    }
}