using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using QuranVideoMaker.CustomControls;
using QuranVideoMaker.Data;
using QuranVideoMaker.Dialogs;
using QuranVideoMaker.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;

namespace QuranVideoMaker
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
        private Project _currentProject = new DefaultNewProject();
        private string _projectFilePath;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public Project CurrentProject
        {
            get { return _currentProject; }
            set
            {
                if (_currentProject != value)
                {
                    _currentProject = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the project file path.
        /// </summary>
        /// <value>
        /// The project file path.
        /// </value>
        public string ProjectFilePath
        {
            get { return _projectFilePath; }
            set
            {
                if (_projectFilePath != value)
                {
                    _projectFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public static MainWindowViewModel Instance { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            Instance = this;

            Task.Delay(3000).ContinueWith((task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    CheckForUpdates();
                }
            });
        }

        [RelayCommand]
        private void OnNewProject()
        {
            var project = new DefaultNewProject();

            var result = QuranVideoMakerUI.ShowDialog(DialogType.NewProject, project);

            if (result.Result.Value)
            {
                CurrentProject = project;
            }
        }

        [RelayCommand]
        private void OnOpenProject()
        {
            var dlg = new OpenFileDialog()
            {
                Filter = "Quran Video Maker Project|*.qv"
            };

            if (dlg.ShowDialog() == true)
            {
                OpenProject(dlg.FileName);
            }
        }

        [RelayCommand]
        private void OnSaveProject()
        {
            if (string.IsNullOrWhiteSpace(ProjectFilePath))
            {
                OnSaveProjectAs();
            }
            else
            {
                var result = CurrentProject.Save(ProjectFilePath);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                }
            }
        }

        [RelayCommand]
        private void OnSaveProjectAs()
        {
            var dlg = new SaveFileDialog()
            {
                Filter = "Quran Video Maker Project|*.qv"
            };

            if (dlg.ShowDialog() == true)
            {
                var result = CurrentProject.Save(dlg.FileName);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

                ProjectFilePath = dlg.FileName;
            }
        }

        [RelayCommand]
        private void OnExportProject()
        {
            QuranVideoMakerUI.ShowDialog(DialogType.ExportProject, CurrentProject);
        }

        [RelayCommand]
        private void OnQuranAdd()
        {
            QuranVideoMakerUI.ShowDialog(DialogType.AddQuran, CurrentProject, false);
        }

        [RelayCommand]
        private void OnQuranSettings()
        {
            QuranVideoMakerUI.ShowDialog(DialogType.QuranSettings, CurrentProject.QuranSettings);
        }

        [RelayCommand]
        private void OnProjectSettings()
        {
            QuranVideoMakerUI.ShowDialog(DialogType.ProjectSettings, CurrentProject);
        }

        [RelayCommand]
        private void OnExitApplication()
        {
            Environment.Exit(0);
        }

        [RelayCommand]
        private void OnProjectPlay()
        {
            CurrentProject.Play();
        }

        [RelayCommand]
        private void OnProjectFastForward()
        {
            CurrentProject.FastForward();
        }

        [RelayCommand]
        private void OnProjectRewind()
        {
            CurrentProject.Rewind();
        }

        [RelayCommand]
        private void OnProjectStop()
        {
            CurrentProject.Stop();
        }

        [RelayCommand]
        private void AutoVerse()
        {
            if (CurrentProject.SelectedTool == TimelineSelectedTool.AutoVerse)
            {
                CurrentProject.AutoVerse();
            }
        }

        [RelayCommand]
        private void ResizeVerse()
        {
            if (CurrentProject.SelectedTool == TimelineSelectedTool.VerseResizer)
            {
                CurrentProject.AutoVerse();
            }
        }

        [RelayCommand]
        private void SelectTool(TimelineSelectedTool tool)
        {
            CurrentProject.SelectedTool = tool;
        }

        [RelayCommand]
        private void OnAddMedia()
        {
            var dlg = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = $"Media files|{FileFormats.AllSupportedFormatsOpenFileExtensions}"
            };

            if (dlg.ShowDialog() == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    var clip = new ProjectClip(file);
                    CurrentProject.Clips.Add(clip);
                    QuranVideoMakerUI.ShowDialog(DialogType.ClipImport, clip);
                }
            }
        }

        [RelayCommand]
        private void OnRemoveMedia()
        {
            foreach (var clip in CurrentProject.Clips.Where(x => x.IsSelected).ToArray())
            {
                CurrentProject.Clips.Remove(clip);
            }
        }

        [RelayCommand]
        private void ReportIssue()
        {
            Process.Start(new ProcessStartInfo("https://github.com/rebinf/QuranVideoMaker/issues/new") { UseShellExecute = true });
        }

        [RelayCommand]
        private void GoToWebsite()
        {
            Process.Start(new ProcessStartInfo("https://github.com/rebinf/QuranVideoMaker") { UseShellExecute = true });
        }

        [RelayCommand]
        private void About()
        {
            QuranVideoMakerUI.ShowDialog(DialogType.About);
        }

        private void CheckForUpdates()
        {
            UpdateChecker.CheckForUpdates().ContinueWith((task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var result = task.Result;

                    if (result.LatestVersion > UpdateChecker.GetCurrentVersion())
                    {
                        MainWindow.Instance.Dispatcher.Invoke(() =>
                        {
                            if (MessageBox.Show(MainWindow.Instance, "A new version is available. Do you want to go to the download page?", "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo(result.ReleaseInfo.HtmlUrl) { UseShellExecute = true });
                            }
                        });
                    }
                }
            });
        }

        private void OpenProject(string projectFile)
        {
            var result = Project.OpenProject(projectFile);

            if (result.Success)
            {
                CurrentProject = result.Data;
                ProjectFilePath = projectFile;

                foreach (var clip in CurrentProject.Clips)
                {
                    QuranVideoMakerUI.ShowDialog(DialogType.ClipImport, clip);
                }

                CurrentProject.PreviewCurrentFrame();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        public string GetProjectFileHash()
        {
            if (!string.IsNullOrWhiteSpace(ProjectFilePath))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(ProjectFilePath))
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                    }
                }
            }

            return string.Empty;
        }

        public void OnLoaded()
        {

        }

        /// <summary>
        /// Called when public properties changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
