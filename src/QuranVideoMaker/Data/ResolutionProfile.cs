using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Resolution Profile
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("ResolutionProfile")]
    [DisplayName("ResolutionProfile")]
    [DebuggerDisplay("ResolutionProfile {Name}")]
    public class ResolutionProfile : INotifyPropertyChanged
    {
        private string _name;
        private int _width;
        private int _height;
        private int _previewWidth;
        private int _previewHeight;
        private double _fps;

        private static List<ResolutionProfile> _presets;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the preview.
        /// </summary>
        /// <value>
        /// The width of the preview.
        /// </value>
        public int PreviewWidth
        {
            get { return _previewWidth; }
            set
            {
                if (_previewWidth != value)
                {
                    _previewWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the preview.
        /// </summary>
        /// <value>
        /// The height of the preview.
        /// </value>
        public int PreviewHeight
        {
            get { return _previewHeight; }
            set
            {
                if (_previewHeight != value)
                {
                    _previewHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the FPS.
        /// </summary>
        /// <value>
        /// The FPS.
        /// </value>
        public double FPS
        {
            get { return _fps; }
            set
            {
                if (_fps != value)
                {
                    _fps = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the presets.
        /// </summary>
        /// <value>
        /// The presets.
        /// </value>
        public static IEnumerable<ResolutionProfile> Presets
        {
            get
            {
                return _presets ??= new List<ResolutionProfile>()
                {
                    new ResolutionProfile("SD 480 25 fps", 640, 480, 640, 480, 25),
                    new ResolutionProfile("HD 720 25 fps", 1280, 720, 640, 480, 25),
                    new ResolutionProfile("FHD 1080 25 fps", 1920, 1080, 640, 480, 25),
                    new ResolutionProfile("QHD 2048 25 fps", 2048, 1152, 640, 480, 25),
                    new ResolutionProfile("UHD 2160 25 fps", 3840, 2160, 640, 480, 25),
                    new ResolutionProfile("4K DCI 2160 25 fps", 4096, 2160, 640, 480, 25),
                    new ResolutionProfile("Custom", 0, 0, 640, 480, 25)
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionProfile"/> class.
        /// </summary>
        public ResolutionProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionProfile" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fps">The FPS.</param>
        public ResolutionProfile(string name, int width, int height, int previewWidth, int previewHeight, double fps)
        {
            Name = name;
            Width = width;
            Height = height;
            PreviewWidth = previewWidth;
            PreviewHeight = previewHeight;
            FPS = fps;
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
