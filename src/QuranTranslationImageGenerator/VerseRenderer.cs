using HarfBuzzSharp;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System.Drawing;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace QuranTranslationImageGenerator
{
    public static class VerseRenderer
    {
        public static List<SKTypeface> Typefaces { get; private set; } = new List<SKTypeface>();

        public static IEnumerable<string> Fonts
        {
            get
            {
                return SKFontManager.Default.GetFontFamilies().Concat(Typefaces.Select(x => x.FamilyName)).Distinct().OrderBy(x => x);
            }
        }

        static VerseRenderer()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Fonts"), "*.*", SearchOption.AllDirectories))
            {
                var tf = SKFontManager.Default.CreateTypeface(file);

                if (tf != null)
                {
                    Typefaces.Add(tf);
                }
            }
        }

        public static IEnumerable<RenderedVerseInfo> RenderVerses(IEnumerable<VerseInfo> verses, QuranRenderSettings renderSettings)
        {
            var list = new List<RenderedVerseInfo>();

            var quranTypeFace = GetTypeface(renderSettings.ArabicScriptRenderSettings.Font, renderSettings.ArabicScriptRenderSettings.BoldFont, renderSettings.ArabicScriptRenderSettings.ItalicFont);

            var bitmap = new SKBitmap(renderSettings.ImageWidth, renderSettings.ImageHeight);
            var canvas = new SKCanvas(bitmap);
            var arabicScriptShaper = new SKShaper(quranTypeFace);

            var shapers = new Dictionary<string, SKShaper>();
            var paints = new Dictionary<string, TextPaints>
            {
                ["QURAN"] = CreatePaints(renderSettings.ArabicScriptRenderSettings, quranTypeFace)
            };

            foreach (var verse in verses)
            {
                var drawGroups = new List<DrawGroup>();

                var renderedVerse = new RenderedVerseInfo(verse);
                list.Add(renderedVerse);

                if (renderSettings.ShowArabicScript)
                {
                    if (verse.VerseNumber != 0 && !(verse.ChapterNumber == 1 && verse.VerseNumber == 1))
                    {
                        if (verse.VerseText.StartsWith(Quran.UthmaniScript.First().VerseText))
                        {
                            verse.VerseText = verse.VerseText[Quran.UthmaniScript.First().VerseText.Length..].Trim();
                        }
                    }

                    canvas.Clear(renderSettings.BackgroundColor);

                    var quranDrawGroup = new DrawGroup(renderSettings.ArabicScriptRenderSettings);
                    drawGroups.Add(quranDrawGroup);

                    foreach (var line in verse.VerseText.Split(Environment.NewLine))
                    {
                        foreach (var item in SplitText(line, renderSettings.ImageWidth, renderSettings.HorizontalMarginPercentage, arabicScriptShaper, paints["QURAN"].TextPaint))
                        {
                            quranDrawGroup.Draws.Add(new DrawInfo(item.Text, paints["QURAN"].TextPaint, paints["QURAN"].TextShadowPaint, arabicScriptShaper, item.Width, item.Height, renderSettings.ArabicScriptRenderSettings.TextShadow, renderSettings.ArabicScriptRenderSettings.TextShadowOffset));
                        }
                    }
                }

                foreach (var translation in verse.Translations)
                {
                    var translationSettings = renderSettings.GetSettingsById(translation.TypeId);
                    var translationId = translationSettings.Id.ToString();

                    var translationDrawGroup = new DrawGroup(translationSettings);
                    drawGroups.Add(translationDrawGroup);

                    if (!paints.ContainsKey(translationId))
                    {
                        paints[translationId] = CreatePaints(translationSettings, GetTypeface(translationSettings.Font, translationSettings.BoldFont, translationSettings.ItalicFont));
                    }

                    if (translationSettings.IsRightToLeft || translationSettings.IsNonAscii)
                    {
                        shapers[translationId] = new SKShaper(GetTypeface(translationSettings.Font, translationSettings.BoldFont, translationSettings.ItalicFont));

                        foreach (var line in translation.VerseText.Split(Environment.NewLine))
                        {
                            foreach (var item in SplitText(line, renderSettings.ImageWidth, renderSettings.HorizontalMarginPercentage, shapers[translationId], paints[translationId].TextPaint))
                            {
                                translationDrawGroup.Draws.Add(new DrawInfo(item.Text, paints[translationId].TextPaint, paints[translationId].TextShadowPaint, shapers[translationId], item.Width, item.Height, translationSettings.TextShadow, translationSettings.TextShadowOffset));
                            }
                        }
                    }
                    else
                    {
                        foreach (var line in translation.VerseText.Split(Environment.NewLine))
                        {
                            foreach (var item in SplitText(line, renderSettings.ImageWidth, renderSettings.HorizontalMarginPercentage, null, paints[translationId].TextPaint))
                            {
                                translationDrawGroup.Draws.Add(new DrawInfo(item.Text, paints[translationId].TextPaint, paints[translationId].TextShadowPaint, null, item.Width, item.Height, translationSettings.TextShadow, translationSettings.TextShadowOffset));
                            }
                        }
                    }
                }

                float totalHeight = drawGroups.SelectMany(x => x.Draws).Sum(x => x.Height) - renderSettings.GapBetweenVerses * drawGroups.Count;
                float currentX = 0;
                float currentY = (bitmap.Height / 2) - (((totalHeight / 2) + renderSettings.GapBetweenVerses * drawGroups.SelectMany(x => x.Draws).Count()) / 2);

                foreach (var group in drawGroups)
                {
                    foreach (var draw in group.Draws)
                    {
                        currentX = bitmap.Width / 2 - (draw.Width / 2);
                        //currentY = currentY + totalHeight - (draw.Height / 2);

                        var coords = new SKPoint(currentX, currentY);

                        if (draw.HasShadow)
                        {
                            var shadowCoords = new SKPoint(coords.X, coords.Y);
                            shadowCoords.Offset(draw.ShadowOffset);

                            if (draw.Shaper != null)
                            {
                                canvas.DrawShapedText(draw.Shaper, draw.Text, shadowCoords, draw.ShadowPaint);
                            }
                            else
                            {
                                canvas.DrawText(draw.Text, shadowCoords, draw.ShadowPaint);
                            }
                        }

                        if (draw.Shaper != null)
                        {
                            canvas.DrawShapedText(draw.Shaper, draw.Text, coords, draw.Paint);
                        }
                        else
                        {
                            canvas.DrawText(draw.Text, coords, draw.Paint);
                        }

                        if (draw == group.Draws.Last())
                        {
                            currentY += draw.Height + renderSettings.GapBetweenVerses;
                        }
                        else
                        {
                            currentY += draw.Height + group.RenderSettings.GapBetweenLines;
                        }
                    }
                }

                ExportVerse(renderedVerse, bitmap, renderSettings);
            }

            return list;
        }

        private static void ExportVerse(RenderedVerseInfo renderedVerse, SKBitmap bitmap, QuranRenderSettings renderSettings)
        {
            var image = SKImage.FromBitmap(bitmap);

            if (renderSettings.OutputType == OutputType.Bitmap)
            {
                renderedVerse.Bitmap = bitmap;
            }
            else if (renderSettings.OutputType == OutputType.Bytes)
            {
                var data = image.Encode(SKEncodedImageFormat.Png, 100);
                var bytes = data.ToArray();
                renderedVerse.ImageContent = bytes;
            }
            else if (renderSettings.OutputType == OutputType.File)
            {
                var data = image.Encode(SKEncodedImageFormat.Png, 100);
                var bytes = data.ToArray();

                var path = Path.Combine(renderSettings.OutputDirectory, $"{renderedVerse.ChapterNumber}-{renderedVerse.VerseNumber}.png");

                if (!Directory.Exists(renderSettings.OutputDirectory))
                {
                    Directory.CreateDirectory(renderSettings.OutputDirectory);
                }

                File.WriteAllBytes(path, bytes);
                renderedVerse.ImagePath = path;
            }
        }

        private static TextPaints CreatePaints(VerseRenderSettings renderSettings, SKTypeface typeface = null)
        {
            var textPaint = new SKPaint()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                Color = renderSettings.TextColor,
                TextSize = renderSettings.FontSize,
            };

            if (typeface != null)
            {
                textPaint.Typeface = typeface;
            }

            var shadowPaint = textPaint.Clone();
            shadowPaint.Color = renderSettings.TextShadowColor;
            shadowPaint.TextSize = renderSettings.FontSize;

            return new TextPaints()
            {
                TextPaint = textPaint,
                TextShadowPaint = shadowPaint
            };
        }

        private static float GetWidth(string text, SKPaint paint, float fontSize)
        {
            using (var blob = paint.Typeface.OpenStream().ToHarfBuzzBlob())
            using (var hbFace = new Face(blob, 0))
            using (var hbFont = new HarfBuzzSharp.Font(hbFace))
            using (var buffer = new HarfBuzzSharp.Buffer())
            {
                buffer.AddUtf16(text);
                buffer.GuessSegmentProperties();
                hbFont.Shape(buffer);
                hbFont.GetScale(out var xScale, out var yScale);
                var scale = fontSize / xScale;
                return buffer.GlyphPositions.Sum(x => x.XAdvance) * scale;
            }
        }

        private static List<TextSize> SplitText(string text, int maxWidth, float horizontalMarginPercentage, SKShaper shaper, SKPaint paint)
        {
            var marginedWidth = maxWidth - maxWidth * 0.1;

            if (horizontalMarginPercentage != 0)
            {
                marginedWidth = maxWidth - maxWidth * (horizontalMarginPercentage / 100);
            }

            var split = text.SplitAndKeep(" ").ToList();
            var result = new List<TextSize>() { new(split[0], 0, 0) };

            for (int i = 1; i < split.Count; i++)
            {
                var tmp = string.Join("", new string[] { result.Last().Text, split[i] });

                var size = GetSize(tmp, shaper, paint);

                if (size.Width <= marginedWidth)
                {
                    result[^1] = new(tmp, size.Width, size.Height);
                }
                else
                {
                    if (i == split.Count - 1)
                    {
                        size = GetSize(split[i], shaper, paint);
                    }

                    result.Add(new(split[i], size.Width, size.Height));
                }
            }

            var first = result.First();

            if (first.Width == 0)
            {
                var size = GetSize(first.Text, shaper, paint);
                first.Width = size.Width;
                first.Height = size.Height;
            }

            return result;
        }

        private static (float Width, float Height) GetSize(string text, SKShaper shaper, SKPaint paint)
        {
            float height = paint.FontMetrics.CapHeight;

            if (shaper != null)
            {
                var shp = shaper.Shape(text, paint);
                var width = shp.Points.Max(x => x.X);
                return (width, height);
            }
            else
            {
                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);
                return (bounds.Width, height);
            }
        }

        public static SKTypeface GetTypeface(string name, bool bold, bool italic)
        {
            var weight = bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
            var slant = italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

            if (Typefaces.FirstOrDefault(x => (x.FamilyName.Equals(name, StringComparison.OrdinalIgnoreCase) && x.IsBold == bold && x.IsItalic == italic) || (x.FamilyName.Equals(name, StringComparison.OrdinalIgnoreCase))) is SKTypeface tf)
            {
                return tf;
            }

            return SKFontManager.Default.MatchFamily(name, new SKFontStyle(weight, SKFontStyleWidth.Normal, slant));
        }

        private class TextPaints
        {
            public SKPaint TextPaint { get; set; }

            public SKPaint TextShadowPaint { get; set; }

        }

        private class DrawGroup
        {
            public VerseRenderSettings RenderSettings { get; set; }

            public List<DrawInfo> Draws { get; set; } = new List<DrawInfo>();

            public DrawGroup(VerseRenderSettings renderSettings)
            {
                RenderSettings = renderSettings;
            }
        }

        private class DrawInfo
        {
            public string Text { get; set; }

            public SKPaint Paint { get; set; }

            public SKPaint ShadowPaint { get; set; }

            public SKPoint ShadowOffset { get; set; }

            public SKShaper Shaper { get; set; }

            public float Width { get; }

            public float Height { get; }

            public bool HasShadow { get; }

            public DrawInfo(string text, SKPaint paint, SKPaint shadowPaint, SKShaper shaper, float width, float height, bool hasShadow, SKPoint shadowOffset)
            {
                Text = text;
                Paint = paint;
                ShadowPaint = shadowPaint;
                Shaper = shaper;
                Width = width;
                Height = height;
                HasShadow = hasShadow;
                ShadowOffset = shadowOffset;
            }
        }
    }
}

public static class StringExtensions
{
    public static IEnumerable<string> SplitAndKeep(this string s, string seperator)
    {
        string[] obj = s.Split(new string[] { seperator }, StringSplitOptions.None);

        for (int i = 0; i < obj.Length; i++)
        {
            string result = i == obj.Length - 1 ? obj[i] : obj[i] + seperator;
            yield return result;
        }
    }
}