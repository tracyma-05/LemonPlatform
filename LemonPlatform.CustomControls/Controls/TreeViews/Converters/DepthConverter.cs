using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace LemonPlatform.CustomControls.Controls.TreeViews.Converters
{
    public class DepthConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var depth = (int)value;
            var margin = new Thickness(depth * 20, 0, 0, 0);
            return margin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
