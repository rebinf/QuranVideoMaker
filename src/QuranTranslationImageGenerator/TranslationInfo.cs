using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranTranslationImageGenerator
{
    /// <summary>
    /// TranslationInfo
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("TranslationInfo")]
    [DisplayName("TranslationInfo")]
    [DebuggerDisplay("TranslationInfo")]
    public class TranslationInfo : INotifyPropertyChanged
    {
        private string _name;
        private string _language;
        private VerseRenderSettings _renderSettings = new VerseRenderSettings();
        private List<VerseInfo> _translatedVerses = new List<VerseInfo>();

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
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
        /// Gets or sets the language.
        /// </summary>
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
        /// Gets a value indicating whether text is right to left.
        /// </summary>
        public bool IsRightToLeft
        {
            get { return RenderSettings.IsRightToLeft; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this verse is the arabic script.
        /// </summary>
        public bool IsNonAscii
        {
            get { return RenderSettings.IsNonAscii; }
        }

        /// <summary>
        /// Gets or sets the render settings.
        /// </summary>
        public VerseRenderSettings RenderSettings
        {
            get { return _renderSettings; }
            set
            {
                if (_renderSettings != value)
                {
                    _renderSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the translated verses.
        /// </summary>
        public List<VerseInfo> TranslatedVerses
        {
            get { return _translatedVerses; }
            set
            {
                if (_translatedVerses != value)
                {
                    _translatedVerses = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationInfo"/> class.
        /// </summary>
        public TranslationInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationInfo" /> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="name">The name.</param>
        public TranslationInfo(string language, string name, bool IsRightToLeft, bool isNonAscii)
        {
            Language = language;
            Name = name;
            RenderSettings.IsRightToLeft = IsRightToLeft;
            RenderSettings.IsNonAscii = isNonAscii;
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
