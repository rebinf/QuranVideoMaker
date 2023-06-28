using QuranVideoMaker.CustomControls;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranVideoMaker.Converters
{
    public class TimelineSelectedToolVisibilityConverter : MarkupExtension, IValueConverter
    {
        public TimelineSelectedTool SelectedTool { get; set; }

        public bool IsVisible { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TimelineSelectedTool)value == SelectedTool ? (IsVisible ? Visibility.Visible : Visibility.Collapsed) : (IsVisible ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? SelectedTool : TimelineSelectedTool.SelectionTool;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
