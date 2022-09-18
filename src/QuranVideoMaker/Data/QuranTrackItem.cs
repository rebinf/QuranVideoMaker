using QuranTranslationImageGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// QuranTrackItem
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("QuranTrackItem")]
    [DisplayName("QuranTrackItem")]
    [DebuggerDisplay("QuranTrackItem")]
    public class QuranTrackItem : TrackItemBase
    {
        private VerseInfo _verse;

        /// <summary>
        /// Gets or sets the verse.
        /// </summary>
        /// <value>
        /// The verse.
        /// </value>
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
    }
}
