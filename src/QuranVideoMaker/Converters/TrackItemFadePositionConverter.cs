using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using QuranVideoMaker.Data;

namespace QuranVideoMaker.Converters
{
    public class TrackItemFadePositionConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var project = values[0] as Project;
            var item = values[2] as ITrackItem;

            if (targetType == typeof(double))
            {
                var pos = (FadeControlType)parameter == FadeControlType.Left ? item.GetFadeInPosition(project.TimelineZoom) : item.GetFadeOutPosition(project.TimelineZoom);

                return pos;
            }

            if (targetType == typeof(PointCollection))
            {
                var collection = new PointCollection();

                if ((FadeControlType)parameter == FadeControlType.Left)
                {
                    collection.Add(new System.Windows.Point(0, 0));
                    collection.Add(new System.Windows.Point(0, 48));
                    collection.Add(new System.Windows.Point(item.GetFadeInPosition(project.TimelineZoom), 0));
                }
                else
                {
                    //<Point X="0" Y="0" />
                    //<Point X="0" Y="48" />
                    //<Point X="-10" Y="0" />
                    collection.Add(new System.Windows.Point(0, 0));
                    collection.Add(new System.Windows.Point(0, 48));
                    collection.Add(new System.Windows.Point(item.GetFadeOutPosition(project.TimelineZoom) * -1, 0));
                }

                return collection;
            }

            return null;
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
