using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranTranslationImageGenerator
{
    public class VerseCollection : List<VerseInfo>
    {
        public VerseRenderSettings RenderSettings { get; set; } = new VerseRenderSettings();
    }
}
