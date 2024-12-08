using QuranVideoMaker.Data;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranVideoMaker.Converters
{
    public class StringToTimeCodeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeCode timeCode)
            {
                return timeCode.ToString();
            }

            if (value is string str)
            {
                if (TimeCode.TryParse(str, out TimeCode tc))
                {
                    return tc;
                }
            }

            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TimeCode timeCode)
            {
                return timeCode.ToString();
            }

            if (value is string str)
            {
                if (TimeCode.TryParse(str, out TimeCode tc))
                {
                    return tc;
                }
            }

            return null;
        }
        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
