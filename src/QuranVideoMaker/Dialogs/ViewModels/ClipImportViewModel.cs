using QuranVideoMaker.Data;
using System.Windows;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    public class ClipImportViewModel : DialogViewModelBase
    {
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

        public ClipImportViewModel(ProjectClip clip)
        {
            Clip = clip;
            Clip.CacheProgress += Clip_CacheProgress;
            Import();
        }

        private void Import()
        {
            Task.Factory.StartNew(() =>
            {
                Clip.CacheFrames();
            }).ContinueWith(x => Application.Current.Dispatcher.Invoke(() => this.CloseWindow.Invoke(true)));
        }

        private void Clip_CacheProgress(object sender, double e)
        {
            Progress = e;
        }

        public override void OnClosed()
        {
            Clip.CacheProgress -= Clip_CacheProgress;
            base.OnClosed();
        }
    }
}
