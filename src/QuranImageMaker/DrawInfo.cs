using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace QuranImageMaker
{
    /// <summary>
    /// Represents drawing information.
    /// </summary>
    public class DrawInfo
    {
        /// <summary>
        /// Gets or sets the text to be drawn.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the paint used to draw the text.
        /// </summary>
        public SKPaint Paint { get; set; }

        /// <summary>
        /// Gets or sets the paint used to draw the shadow.
        /// </summary>
        public SKPaint ShadowPaint { get; set; }

        /// <summary>
        /// Gets or sets the offset of the shadow.
        /// </summary>
        public SKPoint ShadowOffset { get; set; }

        /// <summary>
        /// Gets or sets the shaper used to shape the text.
        /// </summary>
        public SKShaper Shaper { get; set; }

        /// <summary>
        /// Gets the width of the drawing area.
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Gets the height of the drawing area.
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Gets a value indicating whether the text has a shadow.
        /// </summary>
        public bool HasShadow { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawInfo"/> class.
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="paint">The paint used to draw the text.</param>
        /// <param name="shadowPaint">The paint used to draw the shadow.</param>
        /// <param name="shaper">The shaper used to shape the text.</param>
        /// <param name="width">The width of the drawing area.</param>
        /// <param name="height">The height of the drawing area.</param>
        /// <param name="hasShadow">A value indicating whether the text has a shadow.</param>
        /// <param name="shadowOffset">The offset of the shadow.</param>
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
