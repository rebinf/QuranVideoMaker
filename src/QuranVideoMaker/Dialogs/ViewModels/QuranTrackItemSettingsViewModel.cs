using QuranTranslationImageGenerator;
using QuranVideoMaker.Data;
using System.ComponentModel;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    public class QuranTrackItemSettingsViewModel : DialogViewModelBase
    {
        private VerseInfo _currentVerse;
        private byte[] _previewBitmap;

        public QuranTrackItem QuranTrackItem { get; }
        public Project Project { get; }

        public VerseInfo CurrentVerse
        {
            get { return _currentVerse; }
            set
            {
                if (_currentVerse != value)
                {
                    _currentVerse = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public QuranTrackItemSettingsViewModel(QuranTrackItem quranTrackItem, Project project)
        {
            QuranTrackItem = quranTrackItem;
            Project = project;
            CurrentVerse = quranTrackItem.Verse.Clone();

            AttachEvents();
            UpdatePreview();
        }

        private void AttachEvents()
        {
            CurrentVerse.PropertyChanged += VerseContent_PropertyChanged;

            foreach (var item in CurrentVerse.Translations)
            {
                item.PropertyChanged += VerseContent_PropertyChanged;
            }
        }

        private void DetachEvents()
        {
            CurrentVerse.PropertyChanged -= VerseContent_PropertyChanged;

            foreach (var item in CurrentVerse.Translations)
            {
                item.PropertyChanged -= VerseContent_PropertyChanged;
            }
        }

        private void VerseContent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            var tempSettings = Project.QuranSettings.Clone();

            tempSettings.OutputType = OutputType.Bytes;

            var preview = VerseRenderer.RenderVerses(new[] { CurrentVerse }, tempSettings);

            PreviewBitmap = preview.First().ImageContent;
        }

        public override void OnOK()
        {
            QuranTrackItem.Verse = CurrentVerse;

            base.OnOK();
        }

        public override void OnClosed()
        {
            DetachEvents();
            base.OnClosed();
        }
    }
}
