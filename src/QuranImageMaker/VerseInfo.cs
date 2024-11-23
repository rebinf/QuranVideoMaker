using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranImageMaker
{
    /// <summary>
    /// Verse Info
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("VerseInfo")]
    [DisplayName("VerseInfo")]
    [DebuggerDisplay("VerseInfo")]
    public class VerseInfo : INotifyPropertyChanged
    {
        private Guid _typeId;
        private int _chapterNumber;
        private int _verseNumber;
        private int _versePart;
        private string _verseText;
        private ObservableCollection<VerseInfo> _translations = new ObservableCollection<VerseInfo>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        public Guid TypeId
        {
            get { return _typeId; }
            set
            {
                if (_typeId != value)
                {
                    _typeId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        public int ChapterNumber
        {
            get { return _chapterNumber; }
            set
            {
                if (_chapterNumber != value)
                {
                    _chapterNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int VerseNumber
        {
            get { return _verseNumber; }
            set
            {
                if (_verseNumber != value)
                {
                    _verseNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the verse part.
        /// </summary>
        public int VersePart
        {
            get { return _versePart; }
            set
            {
                if (_versePart != value)
                {
                    _versePart = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the verse text.
        /// </summary>
        public string VerseText
        {
            get { return _verseText; }
            set
            {
                if (_verseText != value)
                {
                    _verseText = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        public ObservableCollection<VerseInfo> Translations
        {
            get { return _translations; }
            set
            {
                if (_translations != value)
                {
                    _translations = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfo" /> class.
        /// </summary>
        public VerseInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseInfo" /> class.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <param name="chapterNumber">The chapter.</param>
        /// <param name="verseNumber">The number.</param>
        /// <param name="verseText">The verse text.</param>
        public VerseInfo(Guid typeId, int chapterNumber, int verseNumber, string verseText)
        {
            TypeId = typeId;
            ChapterNumber = chapterNumber;
            VerseNumber = verseNumber;
            VerseText = verseText;
        }

        public VerseInfo Clone()
        {
            var clone = new VerseInfo()
            {
                ChapterNumber = this.ChapterNumber,
                TypeId = this.TypeId,
                VerseNumber = this.VerseNumber,
                VersePart = this.VersePart,
                VerseText = this.VerseText
            };

            foreach (var t in Translations)
            {
                clone.Translations.Add(t.Clone());
            }

            return clone;
        }

        /// <summary>
        /// Called when public properties changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
