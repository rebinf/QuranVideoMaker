using FFMpegCore;
using NAudio.Wave;
using OpenCvSharp;
using QuranTranslationImageGenerator;
using QuranVideoMaker.Serialization;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media.Media3D;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Project
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("Project")]
    [DisplayName("Project")]
    [DebuggerDisplay("Project")]
    public class Project : INotifyPropertyChanged
    {
        private string _id = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
        private int _previewWidth;
        private int _previewHeight;
        private int _chapter;
        private int _verseFrom;
        private int _verseTo;
        private double _fps = 25;
        private int _timelineZoom = 8;
        private int _trackHeadersWidth = 100;
        private TimeCode _needlePositionTime = new TimeCode(0, 25);
        private ObservableCollection<TimelineTrack> _tracks;
        private ObservableCollection<ProjectClipInfo> _clips = new ObservableCollection<ProjectClipInfo>();
        private string _exportDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        private QuranRenderSettings _quranSettings = new QuranRenderSettings();
        private byte[] _currentPreviewFrame;
        private bool _isPlaying;

        private Timer _playTimer = new Timer() { AutoReset = true, Enabled = false };
        private AudioFileReader _audioReader;
        private WaveOutEvent _outputDevice;

        public event EventHandler<double> ExportProgress;


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when needle position time changed (usually from UI).
        /// </summary>
        public event EventHandler<TimeCode> NeedlePositionTimeChanged;

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
        /// Gets or sets the width of the track headers.
        /// </summary>
        /// <value>
        /// The width of the track headers.
        /// </value>
        public int TrackHeadersWidth
        {
            get { return _trackHeadersWidth; }
            set
            {
                if (_trackHeadersWidth != value)
                {
                    _trackHeadersWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Chapter
        /// </summary>
        public int Chapter
        {
            get { return _chapter; }
            set
            {
                if (_chapter != value)
                {
                    _chapter = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// From verse number
        /// </summary>
        public int VerseFrom
        {
            get { return _verseFrom; }
            set
            {
                if (_verseFrom != value)
                {
                    _verseFrom = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// To verse number
        /// </summary>
        public int VerseTo
        {
            get { return _verseTo; }
            set
            {
                if (_verseTo != value)
                {
                    _verseTo = value;
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
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        public int TimelineZoom
        {
            get { return _timelineZoom; }
            set
            {
                if (_timelineZoom != value)
                {
                    _timelineZoom = Math.Clamp(value, 1, Constants.TimelineZoomMax);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the needle position.
        /// </summary>
        /// <value>
        /// The needle position.
        /// </value>
        public TimeCode NeedlePositionTime
        {
            get { return _needlePositionTime; }
            set
            {
                if (_needlePositionTime != value)
                {
                    _needlePositionTime = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tracks.
        /// </summary>
        /// <value>
        /// The tracks.
        /// </value>
        public ObservableCollection<TimelineTrack> Tracks
        {
            get { return _tracks ??= new ObservableCollection<TimelineTrack>(); }
            set
            {
                if (_tracks != value)
                {
                    _tracks = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the clips.
        /// </summary>
        /// <value>
        /// The clips.
        /// </value>
        public ObservableCollection<ProjectClipInfo> Clips
        {
            get { return _clips; }
            set
            {
                if (_clips != value)
                {
                    _clips = value;
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
            get { return QuranSettings.ImageWidth; }
            set
            {
                QuranSettings.ImageWidth = value;
                OnPropertyChanged();
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
            get { return QuranSettings.ImageHeight; }
            set
            {
                QuranSettings.ImageHeight = value;
                OnPropertyChanged();
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

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the export directory.
        /// </summary>
        /// <value>
        /// The export directory.
        /// </value>
        public string ExportDirectory
        {
            get { return _exportDirectory; }
            set
            {
                if (_exportDirectory != value)
                {
                    _exportDirectory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the quran settings.
        /// </summary>
        /// <value>
        /// The quran settings.
        /// </value>
        public QuranRenderSettings QuranSettings
        {
            get { return _quranSettings ??= new QuranRenderSettings(); }
            set
            {
                if (_quranSettings != value)
                {
                    _quranSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimelineTrack QuranTrack { get { return Tracks.FirstOrDefault(x => x.Type == TrackType.Quran); } }

        public IEnumerable<QuranTrackItem> OrderedVerses { get { return Tracks.FirstOrDefault(x => x.Type == TrackType.Quran)?.Items.Cast<QuranTrackItem>().OrderBy(x => x.Verse.VerseNumber); } }

        /// <summary>
        /// Gets or sets the current image.
        /// </summary>
        /// <value>
        /// The current image.
        /// </value>
        [JsonIgnore]
        public byte[] CurrentPreviewFrame
        {
            get { return _currentPreviewFrame; }
            set
            {
                if (_currentPreviewFrame != value)
                {
                    _currentPreviewFrame = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            _playTimer.Elapsed += PlayTimer_Elapsed;
        }

        /// <summary>
        /// Cuts the item at the specified frame.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="frame">The frame.</param>
        public void CutItem(TrackItemBase item, double frame)
        {
            var cutFrame = frame - item.Position.TotalFrames;
            var oldEnd = item.End;
            item.End = new TimeCode(item.Start.TotalFrames + cutFrame, FPS);
            var track = Tracks.First(x => x.Items.Contains(item));

            var newItemPosition = new TimeCode(frame, FPS);
            var newItemStart = item.End;

            if (item.Type == TrackItemType.Quran)
            {
                var newItem = new QuranTrackItem
                {
                    ClipId = item.ClipId,
                    Name = item.Name,
                    Thumbnail = item.Thumbnail,
                    Position = newItemPosition,
                    SourceLength = item.SourceLength,
                    Start = new TimeCode(),
                    End = oldEnd - item.End,
                    Verse = (item as QuranTrackItem).Verse.Clone(),
                    FadeInFrame = item.FadeInFrame,
                    FadeOutFrame = item.FadeOutFrame
                };

                track.Items.Add(newItem);
            }
            else
            {
                var newItem = new TrackItemBase(item.Type, item.UnlimitedSourceLength, item.ClipId, item.Name, item.Thumbnail, newItemPosition, item.SourceLength, newItemStart, oldEnd);

                track.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Cuts the item at the specified frame.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="frame">The frame.</param>
        public void ResizeQuranItem(TrackItemBase item, double frame)
        {
            if (item.Type == TrackItemType.Quran)
            {
                var cutFrame = frame - item.Position.TotalFrames;
                var oldEnd = item.End;
                item.End = new TimeCode(item.Start.TotalFrames + cutFrame, FPS);
                var track = Tracks.First(x => x.Items.Contains(item));

                var newItemPosition = new TimeCode(frame, FPS);
                var newItemStart = item.End;

                if (Tracks.First(x => x.Type == TrackType.Quran).Items.Cast<QuranTrackItem>().FirstOrDefault(x => x.Verse.VerseNumber == (item as QuranTrackItem).Verse.VerseNumber + 1) is QuranTrackItem right)
                {
                    var oldRight = right.GetRight().TotalFrames;
                    right.Position = newItemPosition;

                    right.End = new TimeCode(oldRight - newItemPosition.TotalFrames, FPS);
                }
            }
        }

        public void AutoVerse()
        {
            var quranTrack = Tracks.First(x => x.Type == TrackType.Quran);
            var items = quranTrack.Items.Cast<QuranTrackItem>();
            var last = items.OrderByDescending(x => x.Verse.VerseNumber).First();

            last.End = new TimeCode(NeedlePositionTime.TotalFrames - last.Position.TotalFrames, FPS);

            var verse = Quran.UthmaniScript.First(x => x.ChapterNumber == last.Verse.ChapterNumber && x.VerseNumber == last.Verse.VerseNumber + 1);

            var verseInfo = new VerseInfo(QuranIds.Quran, verse.ChapterNumber, verse.VerseNumber, verse.VerseText);

            var newItem = new QuranTrackItem()
            {
                UnlimitedSourceLength = true,
                Name = $"{verseInfo.ChapterNumber}:{verseInfo.VerseNumber}.{verseInfo.VersePart}",
                Verse = verseInfo,
                SourceLength = TimeCode.FromSeconds(30, FPS),
                Start = new TimeCode(0, FPS),
                End = TimeCode.FromSeconds(30, FPS),
                Position = last.GetRight(),
                FadeInFrame = 25,
                FadeOutFrame = 25,
            };

            foreach (var translation in QuranSettings.TranslationRenderSettings)
            {
                var translationInfo = Quran.Translations.First(x => x.Id == translation.Id);
                var translatedVerse = translationInfo.GetVerse(verse.ChapterNumber, verse.VerseNumber);
                var translatedVerseInfo = new VerseInfo(translationInfo.Id, translatedVerse.ChapterNumber, translatedVerse.ChapterNumber, translatedVerse.VerseText);
                verseInfo.Translations.Add(translatedVerseInfo);

                if (!QuranSettings.TranslationRenderSettings.Any(x => x.Id == translationInfo.Id))
                {
                    QuranSettings.TranslationRenderSettings.Add(translationInfo.GetVerseRenderSettings());
                }
            }

            quranTrack.Items.Add(newItem);
        }

        /// <summary>
        /// Gets the item render order.
        /// </summary>
        /// <param name="trackItem">The track item.</param>
        /// <returns></returns>
        public int GetItemRenderOrder(TrackItemBase trackItem)
        {
            var track = Tracks.First(x => x.Items.Contains(trackItem));

            if (track.Type == TrackType.Quran)
            {
                return Tracks.Count + 1;
            }

            return Tracks.IndexOf(track) + 1;
        }

        /// <summary>
        /// Gets the total frames.
        /// </summary>
        /// <returns></returns>
        public double GetTotalFrames()
        {
            if (!Tracks.SelectMany(x => x.Items).Any())
            {
                return 0;
            }

            return Tracks.SelectMany(x => x.Items).Max(x => x.GetRight().TotalFrames);
        }

        /// <summary>
        /// Gets the visual track items at frame.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <returns></returns>
        public List<TrackItemBase> GetVisualTrackItemsAtFrame(int frame)
        {
            return this.Tracks.SelectMany(x => x.Items).Where(x => x.Type != TrackItemType.Audio)
                .Where(x => x.Position.TotalFrames <= frame && x.GetRight().TotalFrames >= frame).ToList();
        }

        public List<TrackItemBase> GetAudioTrackItemsAtFrame(int frame)
        {
            return this.Tracks.SelectMany(x => x.Items).Where(x => x.Type == TrackItemType.Audio)
                .Where(x => x.Position.TotalFrames <= frame && x.GetRight().TotalFrames >= frame).ToList();
        }

        /// <summary>
        /// Gets the audio track items.
        /// </summary>
        /// <returns></returns>
        public List<TrackItemBase> GetAudioTrackItems()
        {
            return this.Tracks.SelectMany(x => x.Items).Where(x => x.Type == TrackItemType.Audio).ToList();
        }

        /// <summary>
        /// Opens the project.
        /// </summary>
        /// <param name="projectFile">The project file.</param>
        /// <returns></returns>
        public static OperationResult<Project> OpenProject(string projectFile)
        {
            try
            {
                var project = JsonSerializer.Deserialize<Project>(System.IO.File.ReadAllText(projectFile), JsonSerializationSettings.ProjectSerializationSettings);
                return new OperationResult<Project>(true, string.Empty, project);
            }
            catch (Exception ex)
            {
                return new OperationResult<Project>(false, ex.Message, null);
            }
        }

        /// <summary>
        /// Saves the project.
        /// </summary>
        /// <param name="projectFile">The project file.</param>
        /// <returns></returns>
        public OperationResult Save(string projectFile)
        {
            try
            {
                System.IO.File.WriteAllText(projectFile, JsonSerializer.Serialize(this, JsonSerializationSettings.ProjectSerializationSettings));

                return new OperationResult(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        public void Export(string exportPath)
        {
            var sw = Stopwatch.StartNew();

            var videoExportPath = Path.Combine(Path.GetTempPath(), "QuranVideoMaker", $"project_{Id}_video.mp4");
            var audioExportPath = Path.Combine(Path.GetTempPath(), "QuranVideoMaker", $"project_{Id}_audio.mp3");

            var totalFrames = GetTotalFrames();

            //extract video frames
            var videoItems = Tracks.SelectMany(x => x.Items).Where(x => x.Type == TrackItemType.Video);

            var frames = RenderTimeline(0, Convert.ToInt32(GetTotalFrames()), false);

            Debug.WriteLine($"Elapsed: {sw.Elapsed}");

            var count = 0;

            var width = Width;
            var height = Height;

            using (var writer = new VideoWriter(videoExportPath, FourCC.MP4V, FPS, new OpenCvSharp.Size(width, height), true))
            {
                foreach (var item in frames.OrderBy(x => x.Frame))
                {
                    var mat = Mat.FromImageData(item.Rendered);

                    if (mat.Width != width || mat.Height != height)
                    {
                        mat = mat.Resize(new OpenCvSharp.Size(width, height));
                    }

                    writer.Write(mat);
                    count++;

                    var progress = Math.Round(((double)count / (double)totalFrames) * 100d, 2);
                    ExportProgress?.Invoke(null, progress);
                    Debug.WriteLine($"Progress: {progress}%");
                }
            }

            using (var audioOutStream = new FileStream(audioExportPath, FileMode.Create))
            {
                foreach (var audioItem in GetAudioTrackItems())
                {
                    var clip = Clips.FirstOrDefault(x => x.Id == audioItem.ClipId);

                    using (var reader = new Mp3FileReader(clip.FilePath))
                    {
                        Mp3Frame frame;
                        while ((frame = reader.ReadNextFrame()) != null)
                        {
                            var currentSecond = reader.CurrentTime.TotalSeconds;

                            if (currentSecond >= audioItem.Start.TotalSeconds && currentSecond <= audioItem.End.TotalSeconds)
                            {
                                audioOutStream.Write(frame.RawData, 0, frame.RawData.Length);
                            }
                        }
                    }
                }
            }

            FFMpegCore.FFMpeg.ReplaceAudio(videoExportPath, audioExportPath, exportPath);

            sw.Stop();
            Debug.WriteLine($"Elapsed: {sw.Elapsed}");
        }

        public List<FrameContainer> RenderTimeline(int fromFrame, int toFrame, bool preview)
        {
            var frames = new List<int>();

            for (int i = fromFrame; i < toFrame; i++)
            {
                frames.Add(i);
            }

            return RenderFrames(frames.ToArray(), preview);
        }

        public List<FrameContainer> RenderFrames(int[] frameNumbers, bool preview)
        {
            var sw = Stopwatch.StartNew();

            //extract video frames
            var videoItems = Tracks.SelectMany(x => x.Items).Where(x => x.Type == TrackItemType.Video);

            var frames = new ConcurrentBag<FrameContainer>();
            var renderedVerses = new ConcurrentDictionary<string, SKBitmap>();

            var width = Width;
            var height = Height;

            if (preview)
            {
                width /= 4;
                height /= 4;
            }

            var count = 0;

            Parallel.ForEach(frameNumbers, i =>
            //foreach (var i in frameNumbers)
            {
                var visualItems = GetVisualTrackItemsAtFrame(i);
                var currentFrames = new List<FrameData>();

                foreach (var trackItem in visualItems)
                {
                    var itemFrame = trackItem.GetLocalFrame(i);
                    var itemOrder = GetItemRenderOrder(trackItem);

                    if (itemFrame > 0 && itemFrame >= trackItem.Start.TotalFrames && itemFrame <= trackItem.End.TotalFrames)
                    {
                        if (trackItem.Type == TrackItemType.Video)
                        {
                            var clip = Clips.First(x => x.Id == trackItem.ClipId);
                            currentFrames.Add(new FrameData(clip.FramesCache.First(x => x.Frame == itemFrame).Data, trackItem.GetOpacity(itemFrame), itemOrder));
                        }
                        else if (trackItem.Type == TrackItemType.Image)
                        {
                            var clip = Clips.First(x => x.Id == trackItem.ClipId);
                            currentFrames.Add(new FrameData(clip.FramesCache.First().Data, trackItem.GetOpacity(itemFrame), itemOrder));
                        }
                        else if (trackItem.Type == TrackItemType.Quran)
                        {
                            var verseItem = trackItem as QuranTrackItem;

                            if (!renderedVerses.ContainsKey(trackItem.Id))
                            {
                                var rv = VerseRenderer.RenderVerses(new[] { verseItem.Verse }, QuranSettings);
                                renderedVerses[trackItem.Id] = rv.FirstOrDefault().Bitmap;
                            }

                            currentFrames.Add(new FrameData(renderedVerses[trackItem.Id], trackItem.GetOpacity(itemFrame), itemOrder));
                        }
                    }
                }

                var frameBitmap = new SKBitmap(width, height);

                var frameCanvas = new SKCanvas(frameBitmap);
                frameCanvas.Clear(SKColors.Black);

                foreach (var cf in currentFrames.OrderBy(x => x.Order))
                {
                    var cfBitmap = cf.Bitmap ?? SKBitmap.Decode(cf.Data);

                    if (cfBitmap.Width != width || cfBitmap.Height != height)
                    {
                        cfBitmap = cfBitmap.Resize(new SKSizeI(width, height), SKFilterQuality.High);
                    }

                    if (cf.Opacity != 1)
                    {
                        var alpha = (byte)(255 * cf.Opacity);

                        var textPaint = new SKPaint()
                        {
                            Color = new SKColor(255, 255, 255, alpha)
                        };

                        frameCanvas.DrawBitmap(cfBitmap, new SKRect(0, 0, cfBitmap.Width, cfBitmap.Height), textPaint);
                    }
                    else
                    {
                        frameCanvas.DrawBitmap(cfBitmap, new SKRect(0, 0, cfBitmap.Width, cfBitmap.Height));
                    }
                }

                using (var pixmap = frameBitmap.PeekPixels())
                {
                    using (var data = pixmap.Encode(new SKPngEncoderOptions(SKPngEncoderFilterFlags.NoFilters, 0)))
                    {
                        var frameBytes = data.ToArray();
                        frames.Add(new FrameContainer(i) { Rendered = frameBytes });
                    }
                }

                count++;

                var progress = Math.Round(((double)count / (double)frameNumbers.Length) * 100d, 2);
                ExportProgress?.Invoke(null, progress);
                Debug.WriteLine($"Progress: {progress}%");
            }
            );

            sw.Stop();
            Debug.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");

            return frames.ToList();
        }

        #region [Play]

        public void Play()
        {
            if (IsPlaying)
            {
                Stop();
                return;
            }

            _playTimer.Interval = 1000d / (double)FPS;

            var audioItems = GetAudioTrackItemsAtFrame(Convert.ToInt32(NeedlePositionTime.TotalFrames));
            var first = audioItems.First();
            var clip = Clips.FirstOrDefault(x => x.Id == first.ClipId);

            if (_outputDevice != null)
            {
                _outputDevice?.Stop();
            }

            _audioReader = new AudioFileReader(clip.FilePath)
            {
                CurrentTime = TimeSpan.FromSeconds(NeedlePositionTime.TotalSeconds)
            };

            _outputDevice = new WaveOutEvent();
            _outputDevice.Init(_audioReader);

            _playTimer.Start();
            _outputDevice.Play();

            IsPlaying = true;
        }

        public void FastForward()
        {
            var newTime = new TimeCode(NeedlePositionTime.TotalFrames + (3 * FPS), FPS);
            NeedlePositionTime = newTime;
            RaiseNeedlePositionTimeChanged(newTime);
        }

        public void Rewind()
        {
            var newTime = new TimeCode(NeedlePositionTime.TotalFrames - (3 * FPS), FPS);
            NeedlePositionTime = newTime;
            RaiseNeedlePositionTimeChanged(newTime);
        }

        public void Stop()
        {
            _playTimer?.Stop();
            _outputDevice?.Stop();
            IsPlaying = false;
        }

        private void PlayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (NeedlePositionTime.TotalFrames >= GetTotalFrames())
            {
                Stop();
            }

            Application.Current?.Dispatcher.BeginInvoke(() =>
            {
                NeedlePositionTime = NeedlePositionTime.AddFrames(1);
            });

            PreviewCurrentFrame();
        }

        public void PreviewCurrentFrame()
        {
            var currentFrame = Convert.ToInt32(NeedlePositionTime.TotalFrames);

            var render = RenderFrames(new[] { currentFrame }, true);

            if (render.Count > 0)
            {
                CurrentPreviewFrame = render[0].Rendered;
            }
        }

        #endregion

        public void CacheClipFrames()
        {
            foreach (var clip in Clips)
            {
                clip.CacheFrames();
            }
        }

        /// <summary>
        /// Raises the needle position time changed.
        /// </summary>
        /// <param name="time">The time.</param>
        public void RaiseNeedlePositionTimeChanged(TimeCode time)
        {
            if (_outputDevice != null)
            {
                _audioReader.CurrentTime = TimeSpan.FromSeconds(NeedlePositionTime.TotalSeconds);
            }

            PreviewCurrentFrame();

            NeedlePositionTimeChanged?.Invoke(this, time);
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
