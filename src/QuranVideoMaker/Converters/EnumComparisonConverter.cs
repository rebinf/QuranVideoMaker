using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranVideoMaker.Converters
{
    public class EnumComparisonToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public bool Invert { get; set; }

        public string Value { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(Value))
            {
                return false;
            }

            var result = value.ToString().Equals(Value, StringComparison.InvariantCultureIgnoreCase);

            if (Invert)
            {
                return result ? Visibility.Collapsed : Visibility.Visible;
            }

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
