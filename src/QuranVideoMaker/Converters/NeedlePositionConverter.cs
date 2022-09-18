using QuranVideoMaker.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranVideoMaker.Converters
{
    public class NeedlePositionConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var project = values[0] as Project;

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
