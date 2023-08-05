using CommunityToolkit.Mvvm.Input;
using QuranTranslationImageGenerator;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Dialogs.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

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
		/// <value>
		/// The project.
		/// </value>
		public Project Project { get; }
		public bool AutoVerse { get; }

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
		/// Gets or sets the quran settings.
		/// </summary>
		/// <value>
		/// The quran settings.
		/// </value>
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

			if (IncludeBismillah)
			{
				var bismillah = Quran.UthmaniScript.First();

				if (SelectedChapter.Number == 1 && FromVerse == 1)
				{
					FromVerse = 2;
				}
				else
				{
					bismillah.ChapterNumber = SelectedChapter.Number;
					bismillah.VerseNumber = 0;
				}

				verses.Add(bismillah);
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
					verses = Quran.UthmaniScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber == FromVerse).ToList();
					itemLength = TimeCode.FromSeconds(30, Project.FPS).TotalFrames;
				}
			}
			else
			{
				verses.AddRange(Quran.UthmaniScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber >= FromVerse && x.VerseNumber <= ToVerse));

				var lastItem = audioTrack.Items.OrderByDescending(x => x.Position.TotalFrames).First();
				var lastFrame = lastItem.Position.TotalFrames + lastItem.Duration.TotalFrames;
				itemLength = lastFrame / verses.Count;
			}

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
