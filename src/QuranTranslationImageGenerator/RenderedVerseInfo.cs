using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuranTranslationImageGenerator
{
    /// <summary>
    /// Rendered Verse Info
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("RenderedVerseInfo")]
    [DisplayName("RenderedVerseInfo")]
    [DebuggerDisplay("RenderedVerseInfo")]
    public class RenderedVerseInfo : VerseInfo
    {
        private string _imagePath;
        private byte[] _imageContent;
        private SKBitmap _bitmap;

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>
        /// The image path.
        /// </value>
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the content of the image.
        /// </summary>
        /// <value>
        /// The content of the image.
        /// </value>
        public byte[] ImageContent
        {
            get { return _imageContent; }
            set
            {
                if (_imageContent != value)
                {
                    _imageContent = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the bitmap.
        /// </summary>
        /// <value>
        /// The bitmap.
        /// </value>
        public SKBitmap Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfo" /> class.
        /// </summary>
        public RenderedVerseInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfo" /> class.
        /// </summary>
        /// <param name="chapterNumber">The chapter.</param>
        /// <param name="verseNumber">The number.</param>
        /// <param name="verseText">The verse text.</param>
        /// <param name="isNonAscii">if set to <c>true</c> is non ASCII.</param>
        public RenderedVerseInfo(VerseInfo verseInfo)
        {
            ChapterNumber = verseInfo.ChapterNumber;
            VerseNumber = verseInfo.VerseNumber;
            VerseText = verseInfo.VerseText;
            VersePart = verseInfo.VersePart;
        }
    }
}
