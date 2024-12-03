using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LemonPlatform.Core.Converters
{
    public class WidthHeightValueConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double originalValue && parameter is string parameterString)
            {
                if (originalValue <= 0) return (int)originalValue;
                var data = parameterString.Split('-');
                if (data.Length == 2)
                {
                    if (double.TryParse(data[1], out double margin) && double.TryParse(data[0], out double precent))
                    {
                        return (int)(originalValue - margin) * precent;
                    }
                }
            }

            return (int)value;
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