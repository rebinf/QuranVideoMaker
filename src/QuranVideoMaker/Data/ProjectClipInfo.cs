using FFMpegCore;
using NAudio.Wave;
using OpenCvSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// ProjectClipInfo
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("ProjectClipInfo")]
    [DisplayName("ProjectClipInfo")]
    [DebuggerDisplay("{FileName} {Length}")]
    public class ProjectClipInfo : INotifyPropertyChanged
    {
        private string _id = Guid.NewGuid().ToString().Replace("-", string.Empty);
        private TrackType _trackType;
        private TrackItemType _itemType;
        private string _fileHash;
        private string _filePath;
        private string _thumbnail;

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
        public TrackType TrackType
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
        /// Initializes a new instance of the <see cref="ProjectClipInfo"/> class.
        /// </summary>
        public ProjectClipInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectClipInfo"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public ProjectClipInfo(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectClipInfo"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fps">The FPS.</param>
        public ProjectClipInfo(string filePath, double fps)
        {
            FilePath = filePath;
            FPS = fps;
        }

        private void GetInfo()
        {
            var propertes = GetClipProperties();

            UnlimitedLength = propertes.UnlimitedLength;
            TrackType = propertes.TrackType;
            ItemType = propertes.ItemType;
            Length = propertes.Length;
            FPS = propertes.FPS;
            Width = propertes.Width;
            Height = propertes.Height;
        }

        private void GetThumbnail()
        {
            Task.Factory.StartNew(() =>
            {
                CreateThumbnail();
            });
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

            if (Constants.SupportedVideoFormats.Contains(extension))
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

            if (Constants.SupportedAudioFormats.Contains(extension))
            {
            }

            if (Constants.SupportedImageFormats.Contains(extension))
            {
                File.Copy(FilePath, thumbnailPath);
            }

            Thumbnail = thumbnailPath;
        }

        public ClipProperties GetClipProperties()
        {
            var extension = Path.GetExtension(FilePath);

            if (Constants.SupportedVideoFormats.Contains(extension))
            {
                using (var capture = new VideoCapture(FilePath))
                {
                    return new ClipProperties()
                    {
                        TrackType = TrackType.Video,
                        ItemType = TrackItemType.Video,
                        Length = new TimeCode(capture.FrameCount, capture.Fps),
                        FPS = capture.Fps,
                        Width = capture.FrameWidth,
                        Height = capture.FrameHeight
                    };
                }
            }

            if (Constants.SupportedAudioFormats.Contains(extension))
            {
                using (var reader = new Mp3FileReader(FilePath))
                {
                    return new ClipProperties()
                    {
                        TrackType = TrackType.Audio,
                        ItemType = TrackItemType.Audio,
                        Length = TimeCode.FromSeconds(reader.TotalTime.TotalSeconds, MainWindowViewModel.Instance.CurrentProject.FPS),
                        FPS = 0,
                        Width = 0,
                        Height = 0
                    };
                }
            }

            if (Constants.SupportedImageFormats.Contains(extension))
            {
                var img = System.Drawing.Image.FromFile(FilePath);

                return new ClipProperties()
                {
                    TrackType = TrackType.Video,
                    ItemType = TrackItemType.Image,
                    UnlimitedLength = true,
                    Length = TimeCode.FromSeconds(5, 25),
                    FPS = 25,
                    Width = img.Width,
                    Height = img.Height
                };
            }

            return new ClipProperties();
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
