using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuranVideoMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var tmpDir = Path.Combine(Path.GetTempPath(), "QuranVideoMaker");

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }

            FFMpegCore.GlobalFFOptions.Current.BinaryFolder = Path.Combine(AppContext.BaseDirectory, "ffmpeg");

            base.OnStartup(e);
        }
    }
}
