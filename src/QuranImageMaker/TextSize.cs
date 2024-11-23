namespace QuranImageMaker
{
    public class TextSize
    {
        public string Text { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public TextSize(string text, float width, float height)
        {
            Text = text;
            Width = width;
            Height = height;
        }
    }
}
