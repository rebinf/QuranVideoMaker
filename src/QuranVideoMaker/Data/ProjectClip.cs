using NAudio.Wave;
using OpenCvSharp;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// ProjectClipInfo
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("ProjectClipInfo")]
    [DisplayName("ProjectClipInfo")]
    [DebuggerDisplay("{FileName} {Length}")]
    public class ProjectClip : IProjectClip
    {
        private string _id = Guid.NewGuid().ToString().Replace("-", string.Empty);
        private TimelineTrackType _trackType;
        private TrackItemType _itemType;
        private string _fileHash;
        private string _filePath;
        private string _thumbnail;
        private bool _isSelected;

        private int _width;
        private int _height;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<double> CacheProgress;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TimelineTrackType TrackType
        {
            get { return _trackType; }
            set
            {
                if (_trackType != value)
                {
                    _trackType = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public TrackItemType ItemType
        {
            get { return _itemType; }
            set
            {
                if (_itemType != value)
                {
                    _itemType = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnFilePathChanged();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

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
        /// Gets or sets a value indicating whether this clip is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this clip is selected; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeCode Length { get; private set; }

        public bool UnlimitedLength { get; private set; }

        public double FPS { get; private set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                if (_thumbnail != value)
                {
                    _thumbnail = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonIgnore]
        public string TempFramesCacheFile { get { return Path.Combine(Path.GetTempPath(), "QuranVideoMaker", $"clip_{_id}.cache"); } }

        [JsonIgnore]
        public List<FrameCache> FramesCache { get; } = new List<FrameCache>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectClip"/> class.
        /// </summary>
        public ProjectClip()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectClip"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public ProjectClip(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectClip"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fps">The FPS.</param>
        public ProjectClip(string filePath, double fps)
        {
            FilePath = filePath;
            FPS = fps;
        }

        public bool IsCompatibleWith(TimelineTrackType trackType)
        {
            return trackType switch
            {
                TimelineTrackType.Quran => ItemType == TrackItemType.Quran,
                TimelineTrackType.Video => ItemType == TrackItemType.Video || ItemType == TrackItemType.Image,
                TimelineTrackType.Audio => ItemType == TrackItemType.Audio,
                _ => false,
            };
        }

        private void GetInfo()
        {
            var properties = GetClipProperties();

            UnlimitedLength = properties.UnlimitedLength;
            TrackType = properties.TrackType;
            ItemType = properties.ItemType;
            Length = properties.Length;
            FPS = properties.FPS;
            Width = properties.Width;
            Height = properties.Height;
        }

        private void GetThumbnail()
        {
            Task.Factory.StartNew(CreateThumbnail);
        }

        public string GetFileHash()
        {
            if (string.IsNullOrWhiteSpace(_fileHash))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(FilePath))
                    {
                        _fileHash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                    }
                }
            }

            return _fileHash;
        }

        private void OnFilePathChanged()
        {
            GetInfo();
            GetThumbnail();
        }

        public void CreateThumbnail()
        {
            var extension = Path.GetExtension(FilePath);

            var thumbnailPath = Path.Combine(Path.GetTempPath(), "QuranVideoMaker", $"{GetFileHash()}_thumbnail.png");

            if (File.Exists(thumbnailPath))
            {
                Thumbnail = thumbnailPath;
                return;
            }

            if (FileFormats.SupportedVideoFormats.Contains(extension))
            {
                using (var capture = new VideoCapture(FilePath))
                {
                    var middleFrame = capture.FrameCount / 2;
                    capture.PosFrames = middleFrame;
                    var mat = capture.RetrieveMat();
                    mat = mat.Resize(new Size(80 * 4, 50 * 4));
                    mat.SaveImage(thumbnailPath);
                }
            }

            if (FileFormats.SupportedAudioFormats.Contains(extension))
            {
            }

            if (FileFormats.SupportedImageFormats.Contains(extension))
            {
                File.Copy(FilePath, thumbnailPath);
            }

            Thumbnail = thumbnailPath;
        }

        public ProjectClipMetadata GetClipProperties()
        {
            var extension = Path.GetExtension(FilePath);

            if (FileFormats.SupportedVideoFormats.Contains(extension))
            {
                using (var capture = new VideoCapture(FilePath))
                {
                    return new ProjectClipMetadata()
                    {
                        TrackType = TimelineTrackType.Video,
                        ItemType = TrackItemType.Video,
                        Length = new TimeCode(capture.FrameCount, capture.Fps),
                        FPS = capture.Fps,
                        Width = capture.FrameWidth,
                        Height = capture.FrameHeight
                    };
                }
            }

            if (FileFormats.SupportedAudioFormats.Contains(extension))
            {
                using (var reader = new Mp3FileReader(FilePath))
                {
                    return new ProjectClipMetadata()
                    {
                        TrackType = TimelineTrackType.Audio,
                        ItemType = TrackItemType.Audio,
                        Length = TimeCode.FromSeconds(reader.TotalTime.TotalSeconds, MainWindowViewModel.Instance.CurrentProject.FPS),
                        FPS = 0,
                        Width = 0,
                        Height = 0
                    };
                }
            }

            if (FileFormats.SupportedImageFormats.Contains(extension))
            {
                var img = System.Drawing.Image.FromFile(FilePath);

                return new ProjectClipMetadata()
                {
                    TrackType = TimelineTrackType.Video,
                    ItemType = TrackItemType.Image,
                    UnlimitedLength = true,
                    Length = TimeCode.FromSeconds(5, 25),
                    FPS = 25,
                    Width = img.Width,
                    Height = img.Height
                };
            }

            return new ProjectClipMetadata();
        }

        public void CacheFrames()
        {
            if (ItemType == TrackItemType.Video)
            {
                if (!File.Exists(TempFramesCacheFile))
                {
                    var max = 0d;
                    using (var capture = new VideoCapture(FilePath))
                    {
                        var mat = new Mat();

                        max = capture.FrameCount;

                        while (capture.Read(mat))
                        {
                            FramesCache.Add(new FrameCache(capture.PosFrames, mat.ToBytes(".jpg")));
                            var progress = ((double)capture.PosFrames / max) * 100d;
                            CacheProgress?.Invoke(this, progress);
                        }
                    }

                    File.WriteAllText(TempFramesCacheFile, JsonSerializer.Serialize(FramesCache));
                }
                else
                {
                    var cache = JsonSerializer.Deserialize<List<FrameCache>>(File.ReadAllText(TempFramesCacheFile));
                    FramesCache.AddRange(cache);
                    CacheProgress?.Invoke(this, 100);
                }
            }
            else if (ItemType == TrackItemType.Image)
            {
                FramesCache.Add(new FrameCache(0, File.ReadAllBytes(FilePath)));
            }
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
