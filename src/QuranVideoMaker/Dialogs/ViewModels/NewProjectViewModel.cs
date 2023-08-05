using CommunityToolkit.Mvvm.Input;
using QuranTranslationImageGenerator;
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
		private bool _verseTransitions = true;
		private ObservableCollection<TranslationInfo> _translations = new ObservableCollection<TranslationInfo>();

		/// <summary>
		/// Gets the project.
		/// </summary>
		/// <value>
		/// The project.
		/// </value>
		public Project Project { get; }

		/// <summary>
		/// Gets or sets a value indicating whether to add verses.
		/// </summary>
		/// <value>
		///   <c>true</c> if add verses; otherwise, <c>false</c>.
		/// </value>
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
		/// Gets or sets from verse.
		/// </summary>
		/// <value>
		/// From verse.
		/// </value>
		public int FromVerse
		{
			get { return _fromVerse; }
			set
			{
				if (_fromVerse != value)
				{
					_fromVerse = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Converts to verse.
		/// </summary>
		/// <value>
		/// To verse.
		/// </value>
		public int ToVerse
		{
			get { return _toVerse; }
			set
			{
				if (_toVerse != value)
				{
					_toVerse = value;
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
		/// Gets or sets a value indicating whether to add verse transitions.
		/// </summary>
		/// <value>
		///   <c>true</c> if add verse transitions; otherwise, <c>false</c>.
		/// </value>
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
		/// <value>
		/// The translations.
		/// </value>
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

		public ResolutionProfile SelectedProfile
		{
			get { return _selectedProfile; }
			set
			{
				if (_selectedProfile != value)
				{
					_selectedProfile = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the project directory.
		/// </summary>
		/// <value>
		/// The project directory.
		/// </value>
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
		/// Gets or sets the quran audio file.
		/// </summary>
		/// <value>
		/// The quran audio file.
		/// </value>
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
				Filter = "Audio Files|*.mp3"
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
			Project.Width = SelectedProfile.Width;
			Project.Height = SelectedProfile.Height;
			Project.PreviewWidth = SelectedProfile.PreviewWidth;
			Project.PreviewHeight = SelectedProfile.PreviewHeight;
			Project.FPS = SelectedProfile.FPS;

			Project.QuranSettings.ShowArabicScript = ShowArabicScript;

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

			var verses = Quran.UthmaniScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber >= FromVerse && x.VerseNumber <= ToVerse).ToList();

			if (string.IsNullOrWhiteSpace(QuranAudioFile))
			{
				totalFrames = (double)(verses.Count * (5 * Project.FPS)); //5 seconds for each verse
			}

			if (IncludeBismillah)
			{
				verses.Insert(0, Quran.UthmaniScript.First());
			}

			var quranTrack = Project.Tracks.First(x => x.Type == TimelineTrackType.Quran);

			var itemLength = totalFrames / verses.Count;

			for (int i = 0; i < verses.Count; i++)
			{
				var verse = verses[i];

				var verseInfo = new VerseInfo(QuranIds.Quran, verse.ChapterNumber, verse.VerseNumber, verse.VerseText);

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
