﻿using QuranVideoMaker.CustomControls;
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
    public class TimelineSelectedToolToSelectedConverter : MarkupExtension, IValueConverter
    {
        public TimelineSelectedTool SelectedTool { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TimelineSelectedTool)value == SelectedTool;
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
