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

		/// <summary>
		/// Gets the project.
		/// </summary>
		/// <value>
		/// The project.
		/// </value>
		public Project Project { get; }

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
		/// Initializes a new instance of the <see cref="SelectTranslationViewModel"/> class.
		/// </summary>
		public ProjectSettingsViewModel(Project project)
		{
			Project = project;
			SelectedProfile = ResolutionProfile.Presets.FirstOrDefault(x => x.Width == Project.Width && x.Height == Project.Height);
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
			Project.Width = SelectedProfile.Width;
			Project.Height = SelectedProfile.Height;
			Project.FPS = SelectedProfile.FPS;
			Project.PreviewWidth = SelectedProfile.PreviewWidth;
			Project.PreviewHeight = SelectedProfile.PreviewHeight;

			CloseWindow?.Invoke(true);
		}
	}
}
