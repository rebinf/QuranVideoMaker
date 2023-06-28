using SkiaSharp;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace QuranVideoMaker.Converters
{
    public class StringToSKPointConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SKPoint point)
            {
                return $"{point.X}, {point.Y}";
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return new SKPoint();
                }

                if (str.Contains(','))
                {
                    var split = str.Split(",");

                    int.TryParse(split[0], out int x);
                    int.TryParse(split[1], out int y);

                    return new SKPoint(x, y);
                }
                else
                {
                    int.TryParse(str, out int uniform);
                    return new SKPoint(uniform, uniform);
                }
            }

            return SKPoint.Empty;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
