using QuranImageMaker;
using System.ComponentModel;
using System.Diagnostics;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// QuranTrackItem
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("QuranTrackItem")]
    [DisplayName("QuranTrackItem")]
    [DebuggerDisplay("QuranTrackItem")]
    public class QuranTrackItem : TrackItem
    {
        private VerseInfo _verse;

        /// <summary>
        /// Gets or sets the verse.
        /// </summary>
        public VerseInfo Verse
        {
            get { return _verse; }
            set
            {
                if (_verse != value)
                {
                    _verse = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuranTrackItem"/> class.
        /// </summary>
        public QuranTrackItem()
        {
            Type = TrackItemType.Quran;
            UnlimitedSourceLength = true;
        }

        /// <inheritdoc/>
        public override ITrackItem Clone()
        {
            var clone = base.Clone() as QuranTrackItem;

            clone.Verse = Verse.Clone();

            return clone;
        }

        public void UpdateName()
        {
            Name = $"{Verse.ChapterNumber}:{Verse.VerseNumber}.{Verse.VersePart}";
        }
    }
}
