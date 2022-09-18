using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranTranslationImageGenerator
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
