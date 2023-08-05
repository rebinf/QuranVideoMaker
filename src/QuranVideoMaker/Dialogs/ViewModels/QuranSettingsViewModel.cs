using CommunityToolkit.Mvvm.Input;
using QuranTranslationImageGenerator;
using System.ComponentModel;
using System.Diagnostics;

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

		public QuranRenderSettings Settings { get; }

		/// <summary>
		/// Gets or sets the selected chapter.
		/// </summary>
		/// <value>
		/// The selected chapter.
		/// </value>
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
		/// <value>
		/// The preview verse.
		/// </value>
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
		/// <value>
		/// The preview bitmap.
		/// </value>
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
				Settings.TranslationRenderSettings.Add((result.Data as QuranTranslation).GetVerseRenderSettings());
			}
		}

		[RelayCommand]
		private void RefreshPreview()
		{
			UpdatePreview();
		}

		public override void OnClosed()
		{
			DetachEvents();
			base.OnClosed();
		}

		private void UpdatePreview()
		{
			var verseInfo = Quran.UthmaniScript.First(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber == PreviewVerse).ToVerseInfo();

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
