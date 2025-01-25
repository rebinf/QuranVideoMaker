using QuranVideoMaker.CustomControls;
using QuranVideoMaker.Data;
using QuranVideoMaker.Utilities;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace QuranVideoMaker.Converters
{
    public class AudioTrackItemThumbnailGeneratorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var project = values[0] as Project;
            var trackItem = values[1] as AudioTrackItem;
            var control = values[2] as TrackItemControl;

            if (project == null || trackItem == null)
            {
                return null;
            }

            var clip = project.Clips.FirstOrDefault(c => c.Id == trackItem.ClipId) as ProjectClip;

            var clipWidth = clip.GetWidth(project.TimelineZoom);

            var height = 50;
            var audioThumb = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "QuranVideoMaker", $"{clip.GetFileHash()}_{project.TimelineZoom}_wave.png");

            if (!System.IO.File.Exists(audioThumb))
            {
                if (clipWidth > 0)
                {
                    WaveFormGenerator.Generate((int)clipWidth, height, System.Drawing.Color.Transparent, clip.FilePath, audioThumb);
                }
            }

            // return image source
            if (System.IO.File.Exists(audioThumb))
            {
                return new BitmapImage(new System.Uri(audioThumb));
            }

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
