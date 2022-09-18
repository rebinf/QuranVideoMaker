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
    /// Quran Translation
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("QuranTranslation")]
    [DisplayName("QuranTranslation")]
    [DebuggerDisplay("QuranTranslation")]
    public class QuranTranslation : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _language;
        private string _translator;
        private bool _isRightToLeft;
        private bool _isNonAscii;
        private string _content;
        private string _font;
        private bool _boldFont;
        private bool _italicFont;

        private List<Verse> _verses = new List<Verse>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the translator.
        /// </summary>
        /// <value>
        /// The translator.
        /// </value>
        public string Translator
        {
            get { return _translator; }
            set
            {
                if (_translator != value)
                {
                    _translator = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text is right to left.
        /// </summary>
        /// <value>
        ///   <c>true</c> if text is right to left; otherwise, <c>false</c>.
        /// </value>
        public bool IsRightToLeft
        {
            get { return _isRightToLeft; }
            set
            {
                if (_isRightToLeft != value)
                {
                    _isRightToLeft = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this verse is the arabic script.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this verse is the arabic script; otherwise, <c>false</c>.
        /// </value>
        public bool IsNonAscii
        {
            get { return _isNonAscii; }
            set
            {
                if (_isNonAscii != value)
                {
                    _isNonAscii = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public string Font
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
                    _font = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use bold font.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use bold font; otherwise, <c>false</c>.
        /// </value>
        public bool BoldFont
        {
            get { return _boldFont; }
            set
            {
                if (_boldFont != value)
                {
                    _boldFont = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use italic font.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use italic font; otherwise, <c>false</c>.
        /// </value>
        public bool ItalicFont
        {
            get { return _italicFont; }
            set
            {
                if (_italicFont != value)
                {
                    _italicFont = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the verses.
        /// </summary>
        /// <value>
        /// The verses.
        /// </value>
        public IEnumerable<Verse> Verses
        {
            get
            {
                if (_verses.Count == 0)
                {
                    LoadVerses();
                }

                return _verses;
            }
        }

        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public string Info { get { return $"{Language} - {Name}"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuranTranslation" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        public QuranTranslation(Guid id, string content)
        {
            Id = id;
            Content = content;
        }

        private void LoadVerses()
        {
            Quran.LoadVerses(Id, _verses, Content, IsRightToLeft, IsNonAscii);
        }

        /// <summary>
        /// Gets the verse.
        /// </summary>
        /// <param name="chapter">The chapter.</param>
        /// <param name="verse">The verse.</param>
        /// <returns></returns>
        public Verse GetVerse(int chapter, int verse)
        {
            if (verse == 0)
            {
                return Verses.First();
            }

            return Verses.First(x => x.ChapterNumber == chapter && x.VerseNumber == verse);
        }

        /// <summary>
        /// Gets the verse render settings.
        /// </summary>
        /// <returns></returns>
        public VerseRenderSettings GetVerseRenderSettings()
        {
            return new VerseRenderSettings()
            {
                Id = this.Id,
                BoldFont = this.BoldFont,
                Font = this.Font,
                IsRightToLeft = this.IsRightToLeft,
                IsNonAscii = this.IsNonAscii,
                ItalicFont = this.ItalicFont,
            };
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
