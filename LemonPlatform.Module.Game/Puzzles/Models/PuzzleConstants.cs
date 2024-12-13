using System.Windows.Media;

namespace LemonPlatform.Module.Game.Puzzles.Models
{
    internal class PuzzleConstants
    {
        internal static string[] PuzzleWithWeekData =
        [
            string.Empty, "Jan", "Feb", "Mar", "Apr", "May", "Jun", string.Empty,
            string.Empty, "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", string.Empty,
            string.Empty, "1","2","3","4","5","6","7",
            string.Empty,  "8","9","10","11","12","13","14",
            string.Empty, "15","16","17","18","19","20","21",
            string.Empty, "22","23","24","25","26","27","28",
            string.Empty,  "29","30","31","Sun","Mon","Tue","Wed",
            string.Empty,  string.Empty, string.Empty, string.Empty, string.Empty, "Thu", "Fri", "Sat"
        ];

        internal static string[] PuzzleWithoutWeekData =
        [
            string.Empty,  "Jan", "Feb", "Mar", "Apr", "May", "Jun", string.Empty,
            string.Empty,   "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", string.Empty,
            string.Empty,  "1","2","3","4","5","6","7",
            string.Empty, "8","9","10","11","12","13","14",
            string.Empty,  "15","16","17","18","19","20","21",
            string.Empty,  "22","23","24","25","26","27","28",
            string.Empty, "29","30","31",string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty,  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
        ];

        internal static string[] WeekData =
        [
            "Sun","Mon","Tue","Wed","Thu", "Fri", "Sat"
        ];

        internal static string[] MonthData =
        [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];

        internal static Dictionary<int, Brush> ColorMapping = new Dictionary<int, Brush>
        {
            { 0, Brushes.Red },
            { 1, Brushes.Orange },
            { 2, Brushes.Yellow },
            { 3, Brushes.Green },
            { 4, Brushes.Blue },
            { 5, Brushes.Brown },
            { 6, Brushes.Purple },
            { 7, Brushes.Pink },
            { 8, Brushes.Orchid },
            { 9, Brushes.Cyan },
        };
    }
}