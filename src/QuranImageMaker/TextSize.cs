namespace QuranImageMaker
{
    /// <summary>
    /// Represents the size of a text element.
    /// </summary>
    public class TextSize
    {
        /// <summary>
        /// Gets or sets the text content.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the width of the text.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the text.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSize"/> class with the specified text, width, and height.
        /// </summary>
        /// <param name="text">The text content.</param>
        /// <param name="width">The width of the text.</param>
        /// <param name="height">The height of the text.</param>
        public TextSize(string text, float width, float height)
        {
            Text = text;
            Width = width;
            Height = height;
        }
    }
}
