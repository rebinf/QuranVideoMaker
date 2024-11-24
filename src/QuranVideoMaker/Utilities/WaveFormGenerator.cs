using NAudio.Wave;
using NAudio.WaveFormRenderer;

namespace QuranVideoMaker.Utilities
{
    public static class WaveFormGenerator
    {
        public static void Generate(int width, int height, System.Drawing.Color background, string audioFilePath, string outputFilePath)
        {
            var settings = new StandardWaveFormRendererSettings
            {
                Width = width,
                TopHeight = height / 2,
                BottomHeight = height / 2,
                BackgroundColor = background,
                TopPeakPen = new System.Drawing.Pen(System.Drawing.Color.LightGreen, 1),
                BottomPeakPen = new System.Drawing.Pen(System.Drawing.Color.LightGreen, 1),
            };

            var renderer = new WaveFormRenderer();

            using (var audioFileReader = new AudioFileReader(audioFilePath))
            {
                var img = renderer.Render(audioFileReader, settings);
                img.Save(outputFilePath);
            }
        }
    }
}
