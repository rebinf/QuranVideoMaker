using CommunityToolkit.Mvvm.Input;
using QuranImageMaker;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Dialogs.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker
{
    /// <summary>
    /// Add Quran ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Add Quran ViewModel")]
    [DisplayName("AddQuranViewModel")]
    [DebuggerDisplay("AddQuranViewModel")]
    public partial class AddQuranViewModel : DialogViewModelBase
    {
        private ChapterInfo _selectedChapter;
        private int _fromVerse = 1;
        private int _toVerse = 7;
        private bool _includeBismillah;
        private bool _verseTransitions = true;
        private ObservableCollection<TranslationInfo> _translations = new ObservableCollection<TranslationInfo>();
        private VerseRenderSettings _quranSettings = new VerseRenderSettings()
        {
            Font = "KFGQPC Uthmanic Script HAFS"
        };

        /// <summary>
        /// Gets the project.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Gets a value indicating whether the settings are for auto verse.
        /// </summary>
        public bool AutoVerse { get; }

        /// <summary>
        /// Gets or sets the selected chapter.
        /// </summary>
        public ChapterInfo SelectedChapter
        {
            get { return _selectedChapter; }
            set
            {
                if (_selectedChapter != value)
                {
                    _selectedChapter = value;

                    if (_selectedChapter != null)
                    {
                        FromVerse = 1;
                        ToVerse = _selectedChapter.VersesCount;
                    }

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets from verse.
        /// </summary>
        public int FromVerse
        {
            get { return _fromVerse; }
            set
            {
                if (_fromVerse != value)
                {
                    _fromVerse = value;

                    if (_fromVerse < 1)
                    {
                        _fromVerse = 1;
                    }

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Converts to verse.
        /// </summary>
        public int ToVerse
        {
            get { return _toVerse; }
            set
            {
                if (_toVerse != value)
                {
                    _toVerse = value;

                    if (_toVerse < _fromVerse)
                    {
                        _toVerse = _fromVerse;
                    }

                    if (SelectedChapter != null && _toVerse > SelectedChapter.VersesCount)
                    {
                        _toVerse = SelectedChapter.VersesCount;
                    }

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
        /// Gets or sets a value indicating whether to add verse transitions.
        /// </summary>
        public bool VerseTransitions
        {
            get { return _verseTransitions; }
            set
            {
                if (_verseTransitions != value)
                {
                    _verseTransitions = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Quran settings.
        /// </summary>
        public VerseRenderSettings QuranSettings
        {
            get { return _quranSettings; }
            set
            {
                if (_quranSettings != value)
                {
                    _quranSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        public ObservableCollection<TranslationInfo> Translations
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
        /// Initializes a new instance of the <see cref="AddQuranViewModel" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="autoVerse">if set to <c>true</c> auto verse.</param>
        public AddQuranViewModel(Project project, bool autoVerse)
        {
            Project = project;
            AutoVerse = autoVerse;
            _selectedChapter = Quran.Chapters.FirstOrDefault(x => x.Number == 1);
        }

        public override void OnOK()
        {
            var quranTrack = Project.Tracks.First(x => x.Type == TimelineTrackType.Quran);
            var audioTrack = Project.Tracks.First(x => x.Type == TimelineTrackType.Audio);

            var verses = new List<Verse>();
            var itemLength = 0d;

            if (AutoVerse || (IncludeBismillah && SelectedChapter.Number != 1))
            {
                var bismillah = Quran.QuranScript.First();

                bismillah.ChapterNumber = SelectedChapter.Number;
                bismillah.VerseNumber = 0;
                verses.Insert(0, bismillah);
            }

            if (AutoVerse)
            {
                ToVerse = FromVerse;

                if (IncludeBismillah)
                {
                    itemLength = TimeCode.FromSeconds(5, Project.FPS).TotalFrames;
                }
                else
                {
                    verses = Quran.QuranScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber == FromVerse).ToList();
                    itemLength = TimeCode.FromSeconds(30, Project.FPS).TotalFrames;
                }
            }
            else
            {
                verses.AddRange(Quran.QuranScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber >= FromVerse && x.VerseNumber <= ToVerse));

                if (audioTrack.Items.Count > 0)
                {
                    var lastItem = audioTrack.Items.OrderByDescending(x => x.Position.TotalFrames).First();
                    var lastFrame = lastItem.Position.TotalFrames + lastItem.Duration.TotalFrames;
                    itemLength = lastFrame / verses.Count;
                }
                else
                {
                    itemLength = TimeCode.FromSeconds(5, Project.FPS).TotalFrames;
                }
            }

            for (int i = 0; i < verses.Count; i++)
            {
                var verse = verses[i];

                var verseInfo = new VerseInfo(QuranIds.Quran, verse.ChapterNumber, verse.VerseNumber, verse.VerseText);

                if (Project.QuranSettings.IncludeVerseNumbers && verseInfo.VerseNumber != 0)
                {
                    verseInfo.VerseText = $"{verseInfo.VerseText} {Quran.ToArabicNumbers(verseInfo.VerseNumber)}";
                }

                var newItem = new QuranTrackItem()
                {
                    UnlimitedSourceLength = true,
                    Name = $"{verseInfo.ChapterNumber}:{verseInfo.VerseNumber}.{verseInfo.VersePart}",
                    Verse = verseInfo,
                    //SourceLength = new TimeCode(itemLength, Project.FPS),
                    Start = new TimeCode(0, Project.FPS),
                    End = new TimeCode(itemLength, Project.FPS),
                    Position = new TimeCode(i * itemLength, Project.FPS),
                    FadeInFrame = VerseTransitions ? 25 : 0,
                    FadeOutFrame = VerseTransitions ? 25 : 0,
                };

                foreach (var translation in Translations)
                {
                    var translationInfo = Quran.Translations.First(x => x.Name == translation.Name);
                    var translatedVerse = translationInfo.GetVerse(verse.ChapterNumber, verse.VerseNumber);
                    var translatedVerseInfo = new VerseInfo(translationInfo.Id, translatedVerse.ChapterNumber, translatedVerse.ChapterNumber, translatedVerse.VerseText);
                    verseInfo.Translations.Add(translatedVerseInfo);

                    if (!Project.QuranSettings.TranslationRenderSettings.Any(x => x.Id == translationInfo.Id))
                    {
                        Project.QuranSettings.TranslationRenderSettings.Add(translationInfo.GetVerseRenderSettings());
                    }
                }

                quranTrack.Items.Add(newItem);
            }

            Project.ClearVerseRenderCache();

            CloseWindow?.Invoke(true);
        }

        [RelayCommand]
        private void OnAddNewTranslation()
        {
            var result = QuranVideoMakerUI.ShowDialog(DialogType.SelectTranslation);

            if (result.Result == true && result.Data is QuranTranslation qt)
            {
                var translation = new TranslationInfo(qt.Language, qt.Name, qt.IsRightToLeft, qt.IsNonAscii);

                translation.RenderSettings.Font = qt.Font;
                translation.RenderSettings.BoldFont = qt.BoldFont;
                translation.RenderSettings.ItalicFont = qt.ItalicFont;
                translation.RenderSettings.IsNonAscii = qt.IsNonAscii;
                translation.RenderSettings.IsRightToLeft = qt.IsRightToLeft;

                _translations.Add(translation);
            }
        }
    }
}
