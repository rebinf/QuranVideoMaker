using QuranVideoMaker.Data;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranVideoMaker.Converters
{
    public class NeedlePositionConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not Project project)
            {
                return 0;
            }

            var x = project.NeedlePositionTime.TotalFrames * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[project.TimelineZoom];

            return x;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
