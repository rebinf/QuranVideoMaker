using QuranVideoMaker.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    public class ClipImportViewModel : DialogViewModelBase
    {
        private Stopwatch _importTime;
        private System.Timers.Timer _importTimer;
        private string _elapsed;

        public ProjectClip Clip { get; }

        private double _progress;

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Elapsed
        {
            get { return _elapsed; }
            set
            {
                if (_elapsed != value)
                {
                    _elapsed = value;
                    OnPropertyChanged();
                }
            }
        }

        public ClipImportViewModel(ProjectClip clip)
        {
            Clip = clip;
            Clip.CacheProgress += Clip_CacheProgress;
            _importTimer = new System.Timers.Timer(1000);
            _importTimer.Elapsed += ImportTimer_Elapsed;
            Import();
        }

        private void Import()
        {
            Task.Factory.StartNew(() =>
            {
                _importTime = Stopwatch.StartNew();
                _importTimer.Start();
                Clip.CacheFrames();
            }).ContinueWith(x => Application.Current.Dispatcher.Invoke(() => this.CloseWindow.Invoke(true)));
        }

        private void Clip_CacheProgress(object sender, double e)
        {
            Progress = e;
        }

        private void ImportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed = _importTime.Elapsed.ToString(@"hh\:mm\:ss");
        }

        public override void OnClosed()
        {
            Clip.CacheProgress -= Clip_CacheProgress;
            base.OnClosed();
        }
    }
}
