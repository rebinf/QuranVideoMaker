using CommunityToolkit.Mvvm.Input;
using QuranImageMaker;
using System.ComponentModel;
using System.Diagnostics;
using System.Transactions;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// Quran Settings ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("QuranSettingsViewModel")]
    [DisplayName("QuranSettingsViewModel")]
    [DebuggerDisplay("QuranSettingsViewModel")]
    public partial class QuranSettingsViewModel : DialogViewModelBase
    {
        private ChapterInfo _selectedChapter = Quran.Chapters.First();
        private int _previewVerse = 1;
        private byte[] _previewBitmap;

        /// <summary>
        /// Gets the Quran render settings.
        /// </summary>
        public QuranRenderSettings Settings { get; }

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
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preview verse.
        /// </summary>
        public int PreviewVerse
        {
            get { return _previewVerse; }
            set
            {
                if (_previewVerse != value)
                {
                    _previewVerse = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preview bitmap.
        /// </summary>
        public byte[] PreviewBitmap
        {
            get { return _previewBitmap; }
            set
            {
                if (_previewBitmap != value)
                {
                    _previewBitmap = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuranSettingsViewModel"/> class.
        /// </summary>
        public QuranSettingsViewModel(QuranRenderSettings renderSettings)
        {
            Settings = renderSettings;
            AttachEvents();
            UpdatePreview();
        }

        private void AttachEvents()
        {
            Settings.PropertyChanged += VerseContent_PropertyChanged;

            foreach (var item in Settings.TranslationRenderSettings)
            {
                item.PropertyChanged += VerseContent_PropertyChanged;
            }
        }

        private void DetachEvents()
        {
            Settings.PropertyChanged -= VerseContent_PropertyChanged;

            foreach (var item in Settings.TranslationRenderSettings)
            {
                item.PropertyChanged -= VerseContent_PropertyChanged;
            }
        }

        private void VerseContent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreview();
        }

        [RelayCommand]
        private void AddNewTranslation()
        {
            var result = QuranVideoMakerUI.ShowDialog(DialogType.SelectTranslation);

            if (result.Result == true)
            {
                var settings = (result.Data as QuranTranslation).GetVerseRenderSettings();
                Settings.TranslationRenderSettings.Add((result.Data as QuranTranslation).GetVerseRenderSettings());

                foreach (var verse in MainWindowViewModel.Instance.CurrentProject.GetVerses())
                {
                    var translationInfo = Quran.Translations.First(x => x.Id == settings.Id);
                    var translatedVerse = translationInfo.GetVerse(verse.ChapterNumber, verse.VerseNumber);
                    var translatedVerseInfo = new VerseInfo(translationInfo.Id, translatedVerse.ChapterNumber, translatedVerse.ChapterNumber, translatedVerse.VerseText);
                    verse.Translations.Add(translatedVerseInfo);
                }
            }
        }

        [RelayCommand]
        private void RefreshPreview()
        {
            UpdatePreview();
        }

        public override void OnClosed()
        {
            MainWindowViewModel.Instance.CurrentProject.ClearVerseRenderCache();
            DetachEvents();
            base.OnClosed();
        }

        public void UpdatePreview()
        {
            var verseInfo = Quran.QuranScript.First(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber == PreviewVerse).ToVerseInfo();

            foreach (var t in Settings.TranslationRenderSettings)
            {
                var translation = Quran.GetTranslation(t.Id);
                verseInfo.Translations.Add(translation.GetVerse(verseInfo.ChapterNumber, verseInfo.VerseNumber).ToVerseInfo());
            }

            var tempSettings = Settings.Clone();

            tempSettings.OutputType = OutputType.Bytes;

            var preview = VerseRenderer.RenderVerses(new[] { verseInfo }, tempSettings);

            PreviewBitmap = preview.First().ImageContent;
        }
    }
}
