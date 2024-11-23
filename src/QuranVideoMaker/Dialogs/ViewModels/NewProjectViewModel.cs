using CommunityToolkit.Mvvm.Input;
using QuranImageMaker;
using QuranVideoMaker.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// New Project ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("New Project ViewModel")]
    [DisplayName("NewProjectViewModel")]
    [DebuggerDisplay("NewProjectViewModel")]
    public partial class NewProjectViewModel : DialogViewModelBase
    {
        private bool _addVerses = true;
        private string _projectDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        private string _quranAudioFile;
        private ResolutionProfile _selectedProfile;
        private ChapterInfo _selectedChapter;
        private int _fromVerse = 1;
        private int _toVerse = 7;
        private bool _includeBismillah;
        private bool _showArabicScript = true;
        private QuranScriptType _scriptType = QuranScriptType.Simple;
        private bool _verseTransitions = true;
        private ObservableCollection<TranslationInfo> _translations = new ObservableCollection<TranslationInfo>();

        private int _resolutionWidth;
        private int _resolutionHeight;

        /// <summary>
        /// Gets the project.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to add verses.
        /// </summary>
        public bool AddVerses
        {
            get { return _addVerses; }
            set
            {
                if (_addVerses != value)
                {
                    _addVerses = value;
                    OnPropertyChanged();
                }
            }
        }

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
        /// Gets or sets a value indicating whether to include Arabic script.
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
        /// Gets the selected resolution profile.
        /// </summary>
        public ResolutionProfile SelectedProfile
        {
            get { return _selectedProfile; }
            set
            {
                if (_selectedProfile != value)
                {
                    _selectedProfile = value;

                    if (_selectedProfile?.Width > 0 && _selectedProfile?.Height > 0)
                    {
                        ResolutionWidth = value.Width;
                        ResolutionHeight = value.Height;
                    }

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the project directory.
        /// </summary>
        public string ProjectDirectory
        {
            get { return _projectDirectory; }
            set
            {
                if (_projectDirectory != value)
                {
                    _projectDirectory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Quran audio file.
        /// </summary>
        public string QuranAudioFile
        {
            get { return _quranAudioFile; }
            set
            {
                if (_quranAudioFile != value)
                {
                    _quranAudioFile = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the resolution width.
        /// </summary>
        public int ResolutionWidth
        {
            get { return _resolutionWidth; }
            set
            {
                if (_resolutionWidth != value)
                {
                    _resolutionWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the resolution height.
        /// </summary>
        public int ResolutionHeight
        {
            get { return _resolutionHeight; }
            set
            {
                if (_resolutionHeight != value)
                {
                    _resolutionHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectTranslationViewModel"/> class.
        /// </summary>
        public NewProjectViewModel(Project project)
        {
            Project = project;
            SelectedProfile = ResolutionProfile.Presets.First(x => x.Name.StartsWith("FHD"));
            _selectedChapter = Quran.Chapters.FirstOrDefault(x => x.Number == 1);
        }

        [RelayCommand]
        private void OnBrowseProjectDirectory()
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ProjectDirectory = dlg.SelectedPath;
            }
        }

        [RelayCommand]
        private void OnBrowseQuranAudioFile()
        {
            var dlg = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = $"Audio files|{FileFormats.AllSupportedOpenAudioFileExtensions}"
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                QuranAudioFile = dlg.FileName;
            }
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

        public override void OnOK()
        {
            Project.Width = ResolutionWidth;
            Project.Height = ResolutionHeight;
            Project.PreviewWidth = SelectedProfile.PreviewWidth;
            Project.PreviewHeight = SelectedProfile.PreviewHeight;
            Project.FPS = SelectedProfile.FPS;

            Project.QuranSettings.ShowArabicScript = ShowArabicScript;
            Project.QuranSettings.ScriptType = ScriptType;

            var audioTrack = Project.Tracks.First(x => x.Type == TimelineTrackType.Audio);

            var totalFrames = 0d;

            if (!string.IsNullOrWhiteSpace(QuranAudioFile))
            {
                var clip = new ProjectClip(QuranAudioFile, Project.FPS);
                totalFrames = clip.Length.TotalFrames;
                Project.Clips.Add(clip);

                audioTrack.Items.Add(new TrackItem(clip, new TimeCode(0, Project.FPS), new TimeCode(0, Project.FPS), clip.Length));
            }

            if (!AddVerses)
            {
                CloseWindow?.Invoke(true);
                return;
            }

            var verses = Quran.QuranScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber >= FromVerse && x.VerseNumber <= ToVerse).ToList();

            if (string.IsNullOrWhiteSpace(QuranAudioFile))
            {
                totalFrames = (double)(verses.Count * (5 * Project.FPS)); //5 seconds for each verse
            }

            if (IncludeBismillah && SelectedChapter.Number != 1)
            {
                var bismillah = Quran.QuranScript.First();

                bismillah.ChapterNumber = SelectedChapter.Number;
                bismillah.VerseNumber = 0;
                verses.Insert(0, bismillah);
            }

            var quranTrack = Project.Tracks.First(x => x.Type == TimelineTrackType.Quran);

            var itemLength = totalFrames / verses.Count;

            for (int i = 0; i < verses.Count; i++)
            {
                var verse = verses[i];

                var verseInfo = new VerseInfo(QuranIds.Quran, verse.ChapterNumber, verse.VerseNumber, verse.VerseText);

                if (Project.QuranSettings.IncludeVerseNumbers && verseInfo.VerseNumber != 0)
                {
                    verseInfo.VerseText = $"{verseInfo.VerseText}{Quran.ToArabicNumbers(verseInfo.VerseNumber)}";
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

            CloseWindow?.Invoke(true);
        }
    }
}
