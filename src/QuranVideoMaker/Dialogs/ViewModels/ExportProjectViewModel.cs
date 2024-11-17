using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
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
        private Stopwatch _exportTimer = new Stopwatch();
        private double _exportProgress;
        private string _exportProgressMessage;
        private TimeSpan _exportElapsed;
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
        /// Gets or sets the export progress message.
        /// </summary>
        public string ExportProgressMessage
        {
            get { return _exportProgressMessage; }
            set
            {
                if (_exportProgressMessage != value)
                {
                    _exportProgressMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the export elapsed.
        /// </summary>
        public TimeSpan ExportElapsed
        {
            get { return _exportElapsed; }
            set
            {
                if (_exportElapsed != value)
                {
                    _exportElapsed = value;
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

                _exportTimer.Restart();

                var verses = Project.OrderedVerses;

                var fileName = $"Quran.{Project.ExportFormat}";

                if (verses.Any())
                {
                    fileName = $"Quran {verses.First().Verse.ChapterNumber} {verses.First().Verse.VerseNumber}-{verses.Last().Verse.VerseNumber}.{Project.ExportFormat}";
                }

                var filePath = Path.Combine(Project.ExportDirectory, fileName);

                var result = Project.Export(filePath);

                _exportTimer.Stop();

                IsExporting = false;

                if (result.Success)
                {
                    MessageBox.Show("Export Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Export Failed.\r\n{result.Message}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        public override void OnClosed()
        {
            DetachEvents();
            base.OnClosed();
        }

        private void AttachEvents()
        {
            Project.ExportProgress += Project_ExportProgress;
        }
        private void DetachEvents()
        {
            Project.ExportProgress -= Project_ExportProgress;
        }

        private void Project_ExportProgress(object sender, double e)
        {
            ExportProgress = e;
            ExportProgressMessage = Project.ExportProgressMessage;
            ExportElapsed = _exportTimer.Elapsed;
        }

    }
}
