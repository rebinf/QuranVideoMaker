using QuranVideoMaker.CustomControls;
using QuranVideoMaker.Data;
using QuranVideoMaker.Utilities;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuranVideoMaker.Converters
{
    public class AudioTrackItemThumbnailGeneratorConverter : MarkupExtension, IMultiValueConverter
    {
        private static Dictionary<TrackItemControl, double> _lastWidths = new Dictionary<TrackItemControl, double>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var project = values[0] as Project;
            var trackItem = values[1] as AudioTrackItem;
            var control = values[2] as TrackItemControl;

            // convert width to pixels
            var width = control.RenderSize.Width * control.LayoutTransform.Value.M11;
            var lastWidth = 0.0;

            // get last width
            if (_lastWidths.ContainsKey(control))
            {
                lastWidth = _lastWidths[control];
            }
            else
            {
                _lastWidths.Add(control, width);
            }

            if (project == null || trackItem == null)
            {
                return null;
            }

            var clip = project.Clips.FirstOrDefault(c => c.Id == trackItem.ClipId) as ProjectClip;

            var height = 50;
            var audioThumb = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "QuranVideoMaker", $"{clip.GetFileHash()}_{width}_wave.png");

            if (!System.IO.File.Exists(audioThumb))
            {
                if (width > 0)
                {
                    var stream = trackItem.GetWaveStream();

                    if (stream != null)
                    {
                        WaveFormGenerator.Generate((int)width, height, System.Drawing.Color.Transparent, stream, audioThumb);
                    }
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
