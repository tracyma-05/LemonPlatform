using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LemonPlatform.Wpf.Converters
{
    public class TriggerStateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            var enumValue = value.ToString();
            var targetValue = parameter.ToString();

            var isEqual = enumValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);

            return !isEqual;
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