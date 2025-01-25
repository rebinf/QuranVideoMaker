using HarfBuzzSharp;
using QuranImageMaker.Extensions;
using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace QuranImageMaker
{
    /// <summary>
    /// Provides functionality to render verses with specified settings.
    /// </summary>
    public static partial class VerseRenderer
    {
        /// <summary>
        /// Gets the list of typefaces.
        /// </summary>
        public static List<SKTypeface> Typefaces { get; private set; } = new List<SKTypeface>();

        /// <summary>
        /// Gets the available fonts.
        /// </summary>
        public static IEnumerable<string> Fonts
        {
            get
            {
                return SKFontManager.Default.GetFontFamilies().Concat(Typefaces.Select(x => x.FamilyName)).Distinct().OrderBy(x => x);
            }
        }

        /// <summary>
        /// Initializes the <see cref="VerseRenderer"/> class.
        /// </summary>
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

        /// <summary>
        /// Renders the specified verses with the given render settings.
        /// </summary>
        /// <param name="verses">The verses to render.</param>
        /// <param name="renderSettings">The render settings.</param>
        /// <returns>A collection of rendered verse information.</returns>
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
                        if (verse.VerseText.StartsWith(Quran.QuranScript.First().VerseText))
                        {
                            if (verse.VerseText.Length >= Quran.QuranScript.First().VerseText.Length)
                            {
                                verse.VerseText = verse.VerseText[Quran.QuranScript.First().VerseText.Length..].Trim();
                            }
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

                    // render if rendered translation guid is either empty or matches the current translation guid
                    if (renderSettings.RenderedTranslation != Guid.Empty && renderSettings.RenderedTranslation != translationSettings.Id)
                    {
                        continue;
                    }

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

                // does the text has a background color?
                if (renderSettings.TextBackground && !renderSettings.FullScreenTextBackground)
                {
                    var w = 0f;
                    var h = 0f;
                    var x = 0f;
                    var y = 0f;

                    var padding = Convert.ToSingle(renderSettings.TextBackgroundPadding);

                    w = drawGroups.SelectMany(x => x.Draws).Max(x => x.Width) + padding * 2;
                    h = drawGroups.SelectMany(x => x.Draws).Sum(x => x.Height) + padding * 2 + renderSettings.GapBetweenVerses * drawGroups.SelectMany(x => x.Draws).Count();

                    // add default padding
                    var defaultPadding = 100;
                    w += defaultPadding;
                    h += defaultPadding;

                    x = (bitmap.Width - w) / 2;
                    y = (bitmap.Height - h) / 2;

                    y -= defaultPadding / 2;

                    var rect = SKRect.Create(x, y, w, h);

                    var textBackgroundPaint = new SKPaint();

                    // what's the color opacity?
                    if (renderSettings.TextBackgroundOpacity > 0)
                    {
                        // convert the float opacity to byte
                        var opacity = Convert.ToByte(renderSettings.TextBackgroundOpacity * 255);

                        textBackgroundPaint.Color = new SKColor(renderSettings.TextBackgroundColor.Red, renderSettings.TextBackgroundColor.Green, renderSettings.TextBackgroundColor.Blue, opacity);
                    }
                    else
                    {
                        textBackgroundPaint.Color = renderSettings.TextBackgroundColor;
                    }

                    canvas.DrawRect(rect, textBackgroundPaint);
                }

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

        /// <summary>
        /// Exports the rendered verse to the specified output type.
        /// </summary>
        /// <param name="renderedVerse">The rendered verse information.</param>
        /// <param name="bitmap">The bitmap containing the rendered verse.</param>
        /// <param name="renderSettings">The render settings.</param>
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

                var path = string.Empty;

                if (renderedVerse.VersePart != 0)
                {
                    path = Path.Combine(renderSettings.OutputDirectory, $"{renderedVerse.ChapterNumber}-{renderedVerse.VerseNumber}.{renderedVerse.VersePart}.png");
                }
                else
                {
                    path = Path.Combine(renderSettings.OutputDirectory, $"{renderedVerse.ChapterNumber}-{renderedVerse.VerseNumber}.png");
                }

                if (!Directory.Exists(renderSettings.OutputDirectory))
                {
                    Directory.CreateDirectory(renderSettings.OutputDirectory);
                }

                File.WriteAllBytes(path, bytes);
                renderedVerse.ImagePath = path;
            }
        }

        /// <summary>
        /// Creates the text paints for the specified render settings.
        /// </summary>
        /// <param name="renderSettings">The render settings.</param>
        /// <param name="typeface">The typeface to use.</param>
        /// <returns>The created text paints.</returns>
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

        /// <summary>
        /// Gets the width of the specified text.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="paint">The paint to use for measuring.</param>
        /// <param name="fontSize">The font size.</param>
        /// <returns>The width of the text.</returns>
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

        /// <summary>
        /// Splits the text into multiple lines based on the specified width and margin.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="horizontalMarginPercentage">The horizontal margin percentage.</param>
        /// <param name="shaper">The shaper to use.</param>
        /// <param name="paint">The paint to use.</param>
        /// <returns>A list of text sizes representing the split text.</returns>
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

        /// <summary>
        /// Gets the size of the specified text.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="shaper">The shaper to use.</param>
        /// <param name="paint">The paint to use.</param>
        /// <returns>The width and height of the text.</returns>
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

        /// <summary>
        /// Gets the typeface based on the specified font name, bold, and italic settings.
        /// </summary>
        /// <param name="name">The font name.</param>
        /// <param name="bold">if set to <c>true</c> use bold font.</param>
        /// <param name="italic">if set to <c>true</c> use italic font.</param>
        /// <returns>The matching typeface.</returns>
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
    }
}