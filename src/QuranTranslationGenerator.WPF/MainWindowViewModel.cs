﻿using CommunityToolkit.Mvvm.Input;
using QuranTranslationImageGenerator;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace QuranTranslationGenerator.WPF
{
    /// <summary>
    /// MainWindowViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("MainWindowViewModel")]
    [DisplayName("MainWindowViewModel")]
    [DebuggerDisplay("MainWindowViewModel")]
    public partial class MainWindowViewModel : INotifyPropertyChanged
    {
        private ChapterInfo _selectedChapter;
        private int _fromVerse = 1;
        private int _toVerse = 7;
        private int _previewVerse = 2;
        private byte[] _previewBitmap;

        private QuranRenderSettings _settings = new QuranRenderSettings()
        {
            BackgroundColor = SKColors.Transparent,
            OutputType = OutputType.File,
            OutputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Quran")
        };

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public QuranRenderSettings Settings
        {
            get { return _settings; }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
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
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            Settings.TranslationRenderSettings.Add(Quran.GetTranslation(QuranIds.EnglishSaheehInternational).GetVerseRenderSettings());
            Settings.TranslationRenderSettings.Add(Quran.GetTranslation(QuranIds.KurdishTafsiriAsan).GetVerseRenderSettings());

            _selectedChapter = Quran.Chapters.FirstOrDefault(x => x.Number == 1);

            PreviewVerse = 1;

            _settings.PropertyChanged += Settings_PropertyChanged;
        }

        [RelayCommand]
        private void Export()
        {
            var verses = new List<VerseInfo>();

            if (Settings.IncludeBismillah && SelectedChapter.Number != 1)
            {
                var bismillah = Quran.QuranScript.First().ToVerseInfo();
                verses.Add(bismillah);

                foreach (var t in Settings.TranslationRenderSettings)
                {
                    var translation = Quran.GetTranslation(t.Id);
                    bismillah.Translations.Add(translation.GetVerse(bismillah.ChapterNumber, bismillah.VerseNumber).ToVerseInfo());
                }
            }

            foreach (var verseInfo in Quran.QuranScript.Where(x => x.ChapterNumber == SelectedChapter.Number && x.VerseNumber >= FromVerse && x.VerseNumber <= ToVerse).Select(x => x.ToVerseInfo()))
            {
                verses.Add(verseInfo);

                foreach (var t in Settings.TranslationRenderSettings)
                {
                    var translation = Quran.GetTranslation(t.Id);
                    verseInfo.Translations.Add(translation.GetVerse(verseInfo.ChapterNumber, verseInfo.VerseNumber).ToVerseInfo());
                }
            }

            VerseRenderer.RenderVerses(verses, Settings);

            MessageBox.Show("Done.");
        }

        [RelayCommand]
        private void AddNewTranslation()
        {
            var dlg = new SelectTranslationWindow();

            if (dlg.ShowDialog() == true)
            {
                if (dlg.SelectedTranslation != null)
                {
                    Settings.TranslationRenderSettings.Add(dlg.SelectedTranslation.GetVerseRenderSettings());
                }
            }
        }

        [RelayCommand]
        private void RefreshPreview()
        {
            UpdatePreview();
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
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

        /// <summary>
        /// Called when public properties changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (name != nameof(PreviewBitmap))
            {
                UpdatePreview();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
