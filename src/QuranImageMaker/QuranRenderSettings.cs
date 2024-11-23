using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranImageMaker
{
    /// <summary>
    /// Quran Render Settings
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("QuranRenderSettings")]
    [DisplayName("QuranRenderSettings")]
    [DebuggerDisplay("QuranRenderSettings")]
    public class QuranRenderSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private string _outputDirectory = Path.Combine(Path.GetTempPath(), "Quran");
        private OutputType _outputLocation = OutputType.Bitmap;
        private int _imageWidth = 3840;
        private int _imageHeight = 2160;
        private SKColor _backgroundColor = SKColors.Transparent;
        private TextPosition _textPosition;
        private int _gapBetweenVerses = 100;
        private float _horizontalMarginPercentage = 20f;
        private bool _includeBismillah;
        public bool _includeVerseNumbers;
        private bool _showArabicScript = true;
        private bool _textBackground;
        private QuranScriptType _scriptType = QuranScriptType.Simple;

        private SKColor _textBackgroundColor = SKColors.Black;
        private double _textBackgroundOpacity = 0.5;
        private double _textBackgroundPadding = 100;
        private bool _textBackgroundTransition = true;
        private bool _fullScreenTextBackground;

        private VerseRenderSettings _arabicScriptRenderSettings = new VerseRenderSettings()
        {
            Id = QuranIds.Quran,
            Font = "KFGQPC Uthmanic Script HAFS",
            FontSize = 200,
            GapBetweenLines = 150
        };

        private ObservableCollection<VerseRenderSettings> _translationRenderSettings = new ObservableCollection<VerseRenderSettings>();

        /// <summary>
        /// Gets or sets the width of the screen.
        /// </summary>
        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (_imageWidth != value)
                {
                    _imageWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the screen.
        /// </summary>
        public int ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                if (_imageHeight != value)
                {
                    _imageHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        public SKColor BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text position.
        /// </summary>
        public TextPosition TextPosition
        {
            get { return _textPosition; }
            set
            {
                if (_textPosition != value)
                {
                    _textPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include bismillah.
        /// </summary>
        public bool IncludeBismillah
        {
            get { return _includeBismillah; }
            set
            {
                if (_includeBismillah != value)
                {
                    _includeBismillah = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include verse numbers.
        /// </summary>
        public bool IncludeVerseNumbers
        {
            get { return _includeVerseNumbers; }
            set
            {
                if (_includeVerseNumbers != value)
                {
                    _includeVerseNumbers = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include arabic script.
        /// </summary>
        public bool ShowArabicScript
        {
            get { return _showArabicScript; }
            set
            {
                if (_showArabicScript != value)
                {
                    _showArabicScript = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the gap between verses.
        /// </summary>
        public int GapBetweenVerses
        {
            get { return _gapBetweenVerses; }
            set
            {
                if (_gapBetweenVerses != value)
                {
                    _gapBetweenVerses = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal margin percentage.
        /// </summary>
        public float HorizontalMarginPercentage
        {
            get { return _horizontalMarginPercentage; }
            set
            {
                if (_horizontalMarginPercentage != value)
                {
                    _horizontalMarginPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show text background.
        /// </summary>
        public bool TextBackground
        {
            get { return _textBackground; }
            set
            {
                if (_textBackground != value)
                {
                    _textBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text background color.
        /// </summary>
        public SKColor TextBackgroundColor
        {
            get { return _textBackgroundColor; }
            set
            {
                if (_textBackgroundColor != value)
                {
                    _textBackgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text background opacity.
        /// </summary>
        public double TextBackgroundOpacity
        {
            get { return _textBackgroundOpacity; }
            set
            {
                if (_textBackgroundOpacity != value)
                {
                    _textBackgroundOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text background padding.
        /// </summary>
        public double TextBackgroundPadding
        {
            get { return _textBackgroundPadding; }
            set
            {
                if (_textBackgroundPadding != value)
                {
                    _textBackgroundPadding = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show text background transition.
        /// Only applicable when FullScreenTextBackground is true.
        /// </summary>
        public bool TextBackgroundTransition
        {
            get { return _textBackgroundTransition; }
            set
            {
                if (_textBackgroundTransition != value)
                {
                    _textBackgroundTransition = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to render text background full screen.
        /// </summary>
        public bool FullScreenTextBackground
        {
            get { return _fullScreenTextBackground; }
            set
            {
                if (_fullScreenTextBackground != value)
                {
                    _fullScreenTextBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the script type.
        /// </summary>
        public QuranScriptType ScriptType
        {
            get { return _scriptType; }
            set
            {
                if (_scriptType != value)
                {
                    _scriptType = value;
                    Quran.LoadQuranScript(value);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set
            {
                if (_outputDirectory != value)
                {
                    _outputDirectory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the output storage location.
        /// </summary>
        public OutputType OutputType
        {
            get { return _outputLocation; }
            set
            {
                if (_outputLocation != value)
                {
                    _outputLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        public VerseRenderSettings ArabicScriptRenderSettings
        {
            get { return _arabicScriptRenderSettings; }
            set
            {
                if (_arabicScriptRenderSettings != value)
                {
                    _arabicScriptRenderSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<VerseRenderSettings> TranslationRenderSettings
        {
            get { return _translationRenderSettings; }
            set
            {
                if (_translationRenderSettings != value)
                {
                    _translationRenderSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerseRenderSettings"/> class.
        /// </summary>
        public QuranRenderSettings()
        {
            _arabicScriptRenderSettings.PropertyChanged += QuranRenderSettings_PropertyChanged;
            _translationRenderSettings.CollectionChanged += TranslationRenderSettings_CollectionChanged;
        }

        /// <summary>
        /// Gets the settings by identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public VerseRenderSettings GetSettingsById(Guid guid)
        {
            if (guid == QuranIds.Quran)
            {
                return ArabicScriptRenderSettings;
            }

            return TranslationRenderSettings.First(x => x.Id == guid);
        }

        public QuranRenderSettings Clone()
        {
            var clone = new QuranRenderSettings()
            {
                BackgroundColor = this.BackgroundColor,
                TextPosition = this.TextPosition,
                OutputDirectory = this.OutputDirectory,
                ImageHeight = this.ImageHeight,
                ImageWidth = this.ImageWidth,
                HorizontalMarginPercentage = this.HorizontalMarginPercentage,
                GapBetweenVerses = this.GapBetweenVerses,
                IncludeBismillah = this.IncludeBismillah,
                ShowArabicScript = this.ShowArabicScript,
                OutputType = this.OutputType,
                ArabicScriptRenderSettings = this.ArabicScriptRenderSettings.Clone(),
                TextBackground = this.TextBackground,
                TextBackgroundColor = this.TextBackgroundColor,
                TextBackgroundOpacity = this.TextBackgroundOpacity,
                TextBackgroundPadding = this.TextBackgroundPadding,
                TextBackgroundTransition = this.TextBackgroundTransition,
                FullScreenTextBackground = this.FullScreenTextBackground,
            };

            foreach (var ts in TranslationRenderSettings)
            {
                clone.TranslationRenderSettings.Add(ts.Clone());
            }

            return clone;
        }

        private void QuranRenderSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private void TranslationRenderSettings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TranslationRenderSettings));
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
