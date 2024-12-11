using System.Windows.Media;

namespace LemonPlatform.Module.Game.Puzzles.Models
{
    public class PuzzleItem
    {
        public string? Content { get; set; }

        public Brush Background { get; set; } = null!;
    }
}