using QuranImageMaker;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuranImageMaker.App
{
    public class TranslationGuidToInfoConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Guid guid)
            {
                return Quran.GetTranslation(guid).Info;
            }

            return null;
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
