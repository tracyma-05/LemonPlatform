using LemonPlatform.Module.Game.Puzzles.Models;
using System.Windows;
using System.Windows.Controls;

namespace LemonPlatform.Module.Game.Puzzles.Controls
{
    public class PuzzlePanel : Control
    {
        static PuzzlePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PuzzlePanel), new FrameworkPropertyMetadata(typeof(PuzzlePanel)));
        }

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(PuzzlePanel), new PropertyMetadata(null, OnSelectedDateChanged));

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as PuzzlePanel;
            panel?.RenderCalendar();
        }

        private void RenderCalendar()
        {
            GenerateCalendar(SelectedDate ?? DateTime.Now);
        }

        public IList<DeskModel> PuzzleItems
        {
            get { return (IList<DeskModel>)GetValue(PuzzleItemsProperty); }
            set { SetValue(PuzzleItemsProperty, value); }
        }

        public static readonly DependencyProperty PuzzleItemsProperty =
            DependencyProperty.Register("PuzzleItems", typeof(IList<DeskModel>), typeof(PuzzlePanel), new PropertyMetadata(OnSelectedDateChanged));


        public int Rows 
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(CalendarBox), new PropertyMetadata(8));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(CalendarBox), new PropertyMetadata(8));


        private CalendarBox _calendarBox;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _calendarBox = (CalendarBox)GetTemplateChild("PART_ListBox");
            GenerateCalendar(SelectedDate ?? DateTime.Now);
        }

        private void GenerateCalendar(DateTime current)
        {
            if (PuzzleItems == null || !PuzzleItems.Any() || _calendarBox == null) return;

            _calendarBox.Items.Clear();
            foreach (var item in PuzzleItems)
            {
                var boxItem = new CalendarBoxItem();
                boxItem.Background = item.Background;
                if (!string.IsNullOrEmpty(item.Content))
                {
                    boxItem.Content = item.Content;
                }

                _calendarBox.Items.Add(boxItem);
            }
        }
    }
}