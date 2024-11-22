using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranTranslationImageGenerator
{
    /// <summary>
    /// ChapterInfo
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("ChapterInfo")]
    [DisplayName("ChapterInfo")]
    [DebuggerDisplay("{Number}. {EnglishName}")]
    public class ChapterInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int _number;
        private int _versesCount;
        private string _englishName;
        private string _nameMeaning;
        private string _arabicName;

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the english.
        /// </summary>
        public string EnglishName
        {
            get { return _englishName; }
            set
            {
                if (_englishName != value)
                {
                    _englishName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name meaning.
        /// </summary>
        public string NameMeaning
        {
            get { return _nameMeaning; }
            set
            {
                if (_nameMeaning != value)
                {
                    _nameMeaning = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the arabic.
        /// </summary>
        public string ArabicName
        {
            get { return _arabicName; }
            set
            {
                if (_arabicName != value)
                {
                    _arabicName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the verses count.
        /// </summary>
        public int VersesCount
        {
            get { return _versesCount; }
            set
            {
                if (_versesCount != value)
                {
                    _versesCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Info { get { return $"{Number}. {EnglishName}"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChapterInfo"/> class.
        /// </summary>
        public ChapterInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChapterInfo"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="englishName">Name of the english.</param>
        /// <param name="arabicName">Name of the arabic.</param>
        /// <param name="versesCount">The verses count.</param>
        public ChapterInfo(int number, string englishName, string nameMeaning, string arabicName, int versesCount)
        {
            Number = number;
            EnglishName = englishName;
            NameMeaning = nameMeaning;
            ArabicName = arabicName;
            VersesCount = versesCount;
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
