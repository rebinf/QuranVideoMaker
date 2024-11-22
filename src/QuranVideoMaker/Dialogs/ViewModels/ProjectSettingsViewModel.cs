using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// Project Settings ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Project Settings ViewModel")]
    [DisplayName("ProjectSettingsViewModel")]
    [DebuggerDisplay("ProjectSettingsViewModel")]
    public partial class ProjectSettingsViewModel : DialogViewModelBase
    {
        private ResolutionProfile _selectedProfile;
        private int _resolutionWidth;
        private int _resolutionHeight;

        /// <summary>
        /// Gets the project.
        /// </summary>
        public Project Project { get; }

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
        public ProjectSettingsViewModel(Project project)
        {
            Project = project;
            SelectedProfile = ResolutionProfile.Presets.FirstOrDefault(x => x.Width == Project.Width && x.Height == Project.Height);

            ResolutionWidth = Project.Width;
            ResolutionHeight = Project.Height;
        }

        [RelayCommand]
        private void OnBrowseProjectDirectory()
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Project.ExportDirectory = dlg.SelectedPath;
            }
        }

        public override void OnOK()
        {
            Project.Width = ResolutionWidth;
            Project.Height = ResolutionHeight;

            if (SelectedProfile != null)
            {
                Project.FPS = SelectedProfile.FPS;
                Project.PreviewWidth = SelectedProfile.PreviewWidth;
                Project.PreviewHeight = SelectedProfile.PreviewHeight;
            }

            CloseWindow?.Invoke(true);
        }
    }
}
