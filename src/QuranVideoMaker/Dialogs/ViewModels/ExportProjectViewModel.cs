using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using QuranTranslationImageGenerator;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Dialogs.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuranVideoMaker
{
    /// <summary>
    /// Export Project ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Export Project ViewModel")]
    [DisplayName("ExportProjectViewModel")]
    [DebuggerDisplay("ExportProjectViewModel")]
    public partial class ExportProjectViewModel : DialogViewModelBase
    {
        private double _exportProgress;
        private bool _isExporting;

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public Project Project { get; }

        /// <summary>
        /// Gets or sets the export progress.
        /// </summary>
        /// <value>
        /// The export progress.
        /// </value>
        public double ExportProgress
        {
            get { return _exportProgress; }
            set
            {
                if (_exportProgress != value)
                {
                    _exportProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether project is exporting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if project is exporting; otherwise, <c>false</c>.
        /// </value>
        public bool IsExporting
        {
            get { return _isExporting; }
            set
            {
                if (_isExporting != value)
                {
                    _isExporting = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddQuranViewModel" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="autoVerse">if set to <c>true</c> auto verse.</param>
        public ExportProjectViewModel(Project project)
        {
            Project = project;
            AttachEvents();
        }

        [RelayCommand]
        private void BrowseExportPath()
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Project.ExportDirectory = dlg.SelectedPath;
            }
        }

        public override void OnOK()
        {
            Task.Factory.StartNew(() =>
            {
                IsExporting = true;
                var verses = Project.OrderedVerses;
                var fileName = $"Quran {verses.First().Verse.VerseNumber}-{verses.Last().Verse.VerseNumber}.mp4";
                var filePath = Path.Combine(Project.ExportDirectory, fileName);

                Project.Export(filePath);
                IsExporting = false;
            });
        }

        public override void OnClosed()
        {
            DettachEvents();
            base.OnClosed();
        }

        private void AttachEvents()
        {
            Project.ExportProgress += Project_ExportProgress;
        }
        private void DettachEvents()
        {
            Project.ExportProgress -= Project_ExportProgress;
        }

        private void Project_ExportProgress(object sender, double e)
        {
            ExportProgress = e;
        }

    }
}
