namespace LemonPlatform.Module.Game.Puzzles.Models
{
    internal class PuzzleConstants
    {
        internal static string[] PuzzleWithWeekData =
        [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", string.Empty,
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", string.Empty,
            "1","2","3","4","5","6","7",
            "8","9","10","11","12","13","14",
            "15","16","17","18","19","20","21",
            "22","23","24","25","26","27","28",
            "29","30","31","Sun","Mon","Tue","Wed",
            string.Empty, string.Empty, string.Empty, string.Empty, "Thu", "Fri", "Sat"
        ];

        internal static string[] PuzzleWithoutWeekData =
        [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", string.Empty,
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", string.Empty,
            "1","2","3","4","5","6","7",
            "8","9","10","11","12","13","14",
            "15","16","17","18","19","20","21",
            "22","23","24","25","26","27","28",
            "29","30","31",string.Empty, string.Empty, string.Empty, string.Empty
        ];

        internal static string[] WeekData =
        [
            "Sun","Mon","Tue","Wed","Thu", "Fri", "Sat"
        ];

        internal static string[] MonthData =
        [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
    }
}