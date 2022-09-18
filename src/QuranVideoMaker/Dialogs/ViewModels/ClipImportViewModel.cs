using OpenCvSharp;
using QuranVideoMaker.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuranVideoMaker.Dialogs.ViewModels
{
    public class ClipImportViewModel : DialogViewModelBase
    {
        public ProjectClipInfo Clip { get; }

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

        public ClipImportViewModel(ProjectClipInfo clip)
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
