using SkiaSharp;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace QuranImageMaker.App
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
                var split = str.Split(",");

                int.TryParse(split[0], out int x);
                int.TryParse(split[1], out int y);

                return new SKPoint(x, y);
            }

            return SKPoint.Empty;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
