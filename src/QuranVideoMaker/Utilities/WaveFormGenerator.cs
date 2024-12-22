using NAudio.Wave;
using NAudio.WaveFormRenderer;
using System.IO;

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

        public static void Generate(int width, int height, System.Drawing.Color background, WaveStream waveStream, string outputFilePath)
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

            var img = renderer.Render(waveStream, settings);
            img.Save(outputFilePath);
        }

        public static void Generate(int width, int height, System.Drawing.Color background, byte[] audioData, string outputFilePath)
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

            using (var audioFileReader = new RawSourceWaveStream(new MemoryStream(audioData), new WaveFormat()))
            {
                var img = renderer.Render(audioFileReader, settings);
                img.Save(outputFilePath);
            }
        }
    }
}
