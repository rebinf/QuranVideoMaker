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
        private bool _showArabicScript = true;

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
        /// <value>
        /// The width of the screen.
        /// </value>
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
        /// <value>
        /// The height of the screen.
        /// </value>
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
        /// <value>
        /// The color of the background.
        /// </value>
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
        /// <value>
        /// The text position.
        /// </value>
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
        /// <value>
        ///   <c>true</c> if include bismillah; otherwise, <c>false</c>.
        /// </value>
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
        /// Gets or sets a value indicating whether to include arabic script.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include arabic script; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        /// The gap.
        /// </value>
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
        /// <value>
        /// The horizontal margin percentage.
        /// </value>
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
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
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
        /// <value>
        /// The output storage location.
        /// </value>
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
