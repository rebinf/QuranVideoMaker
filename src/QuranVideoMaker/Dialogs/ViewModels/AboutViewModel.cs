using CommunityToolkit.Mvvm.Input;
using QuranVideoMaker.Utilities;
using System.ComponentModel;
using System.Diagnostics;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    /// <summary>
    /// About ViewModel
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("About ViewModel")]
    [DisplayName("AboutViewModel")]
    [DebuggerDisplay("AboutViewModel")]
    public partial class AboutViewModel : DialogViewModelBase
    {
        public string Version => UpdateChecker.GetCurrentVersion().ToString();

        private string _downloadUrl;

        private bool _updateAvailable;
        private string _latestMessage;

        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
            set
            {
                if (_updateAvailable != value)
                {
                    _updateAvailable = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LatestVersion
        {
            get { return _latestMessage; }
            set
            {
                if (_latestMessage != value)
                {
                    _latestMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DownloadUrl
        {
            get { return _downloadUrl; }
            set
            {
                if (_downloadUrl != value)
                {
                    _downloadUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel()
        {
            UpdateChecker.CheckForUpdates().ContinueWith((task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var result = task.Result;

                    if (result.LatestVersion > UpdateChecker.GetCurrentVersion())
                    {
                        LatestVersion = result.LatestVersion.ToString();
                        DownloadUrl = result.ReleaseInfo.HtmlUrl;
                        UpdateAvailable = true;
                    }
                }
            });
        }

        [RelayCommand]
        private void GoToWebsite()
        {
            Process.Start(new ProcessStartInfo("https://github.com/rebinf/QuranVideoMaker") { UseShellExecute = true });
        }

        [RelayCommand]
        private void GoToDownload()
        {
            Process.Start(new ProcessStartInfo(DownloadUrl) { UseShellExecute = true });
        }

        [RelayCommand]
        private void GoToProfile()
        {
            Process.Start(new ProcessStartInfo("https://github.com/rebinf") { UseShellExecute = true });
        }
    }
}
