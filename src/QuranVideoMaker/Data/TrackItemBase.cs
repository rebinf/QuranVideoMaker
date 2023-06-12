using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// TrackItemBase
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    [Description("TrackItemBase")]
    [DisplayName("TrackItemBase")]
    [DebuggerDisplay("TrackItemBase")]
    public class TrackItemBase : ITrackItem
    {
        private string _id = Guid.NewGuid().ToString().Replace("-", string.Empty);
        private TrackItemType _type;
        private string _clipId;
        private string _name;
        private string _note;
        private TimeCode _position;
        private TimeCode _sourceLength;
        private bool _unlimitedSourceLength;
        private bool _isSelected;
        private bool _ischangingFadeIn;
        private TimeCode _start;
        private TimeCode _end;
        private string _thumbnail;

        private double _fadeInFrame;
        private double _fadeOutFrame;
        private bool _ischangingFadeOut;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public TrackItemType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public string ClipId
        {
            get { return _clipId; }
            set
            {
                if (_clipId != value)
                {
                    _clipId = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public TimeCode SourceLength
        {
            get { return _sourceLength; }
            set
            {
                if (_sourceLength != value)
                {
                    _sourceLength = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public bool UnlimitedSourceLength
        {
            get { return _unlimitedSourceLength; }
            set
            {
                if (_unlimitedSourceLength != value)
                {
                    _unlimitedSourceLength = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public TimeCode Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public TimeCode Start
        {
            get { return _start; }
            set
            {
                if (_start != value)
                {
                    if (!UnlimitedSourceLength)
                    {
                        if (End.TotalFrames > 0 && value >= End)
                        {
                            value = new TimeCode(Convert.ToInt32(End.TotalFrames) - 1, End.FPS);
                        }
                    }
                    else
                    {
                        value = new TimeCode();
                    }

                    if (value <= new TimeCode())
                    {
                        value = new TimeCode();
                    }

                    _start = value;

                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public TimeCode End
        {
            get { return _end; }
            set
            {
                if (_end != value)
                {
                    if (!UnlimitedSourceLength)
                    {
                        if (value >= SourceLength)
                        {
                            value = new TimeCode(SourceLength.TotalFrames, SourceLength.FPS);
                        }
                    }

                    if (value <= Start)
                    {
                        value = new TimeCode(Start.TotalFrames + 1, value.FPS);
                    }

                    _end = value;

                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        [JsonIgnore]
        public bool IsChangingFadeIn
        {
            get { return _ischangingFadeIn; }
            set
            {
                if (_ischangingFadeIn != value)
                {
                    _ischangingFadeIn = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        [JsonIgnore]
        public bool IsChangingFadeOut
        {
            get { return _ischangingFadeOut; }
            set
            {
                if (_ischangingFadeOut != value)
                {
                    _ischangingFadeOut = value;
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public double FadeInFrame
        {
            get { return _fadeInFrame; }
            set
            {
                if (_fadeInFrame != value)
                {
                    if (value < 0)
                    {
                        value = 0;
                    }

                    if (value > GetLength().TotalFrames - 25)
                    {
                        value = GetLength().TotalFrames - 25;
                    }

                    _fadeInFrame = value;
                    Debug.WriteLine($"set: {value}, max({GetLength().TotalFrames})");
                    OnPropertyChanged();
                }
            }
        }

        ///<inheritdoc/>
        public double FadeOutFrame
        {
            get { return _fadeOutFrame; }
            set
            {
                if (_fadeOutFrame != value)
                {
                    if (value < 0)
                    {
                        value = 0;
                    }

                    if (value > GetLength().TotalFrames - 25)
                    {
                        value = GetLength().TotalFrames - 25;
                    }

                    _fadeOutFrame = value;
                    Debug.WriteLine($"set: {value}, max({GetLength().TotalFrames})");
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemBase"/> class.
        /// </summary>
        public TrackItemBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackItemBase" /> class.
        /// </summary>
        /// <param name="unlimitedSourceLength">if set to <c>true</c> [unlimited source length].</param>
        /// <param name="clipId">The clip identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <param name="position">The position.</param>
        /// <param name="sourceLength">Length of the source.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public TrackItemBase(TrackItemType type, bool unlimitedSourceLength, string clipId, string name, string thumbnail, TimeCode position, TimeCode sourceLength, TimeCode start, TimeCode end)
        {
            Type = type;
            UnlimitedSourceLength = unlimitedSourceLength;
            ClipId = clipId;
            Name = name;
            Thumbnail = thumbnail;
            Position = position;
            SourceLength = sourceLength;
            Start = start;
            End = end;
        }

        public TrackItemBase(ProjectClipInfo clipInfo, TimeCode position, TimeCode start, TimeCode end)
        {
            Type = clipInfo.ItemType;
            UnlimitedSourceLength = clipInfo.UnlimitedLength;
            ClipId = clipInfo.Id;
            Name = clipInfo.FileName;
            Thumbnail = clipInfo.Thumbnail;
            Position = position;
            SourceLength = clipInfo.Length;
            Start = start;
            End = end;
        }

        public bool IsCompatibleWith(TrackType trackType)
        {
            switch (trackType)
            {
                case TrackType.Quran:
                    return Type == TrackItemType.Quran;
                case TrackType.Video:
                    return Type == TrackItemType.Video || Type == TrackItemType.Image;
                case TrackType.Audio:
                    return Type == TrackItemType.Audio;
                default:
                    return false;
            }
        }

        public double GetPosition(int zoom)
        {
            var position = Position.TotalFrames * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public double GetPlayPosition(int zoom, double needleFrame)
        {
            var position = GetPosition(zoom);
            var playPosition = needleFrame + Start.Frame;

            return playPosition;
        }

        public double GetFadeInPosition(int zoom)
        {
            var position = FadeInFrame * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public double GetFadeOutPosition(int zoom)
        {
            var position = FadeOutFrame * Constants.TimelinePixelsInSeparator / Constants.TimelineZooms[zoom];
            return position;
        }

        public double GetWidth(int zoom)
        {
            var length = GetLength().TotalFrames / Constants.TimelineZooms[zoom] * Constants.TimelinePixelsInSeparator;
            return length;
        }

        public TimeCode GetLength()
        {
            return End - Start;
        }

        public TimeCode GetRight()
        {
            return Position + GetLength();
        }

        public double GetOpacity(int itemFrame)
        {
            var opacity = 1d;

            if (this.FadeInFrame > 0)
            {
                if (itemFrame < this.FadeInFrame)
                {
                    opacity = Math.Clamp(itemFrame * (1.0 / this.FadeInFrame), 0, 1);
                }
            }

            if (this.FadeOutFrame > 0)
            {
                var itemRightFrame = this.GetRight().TotalFrames - this.Position.TotalFrames;

                if (itemFrame > itemRightFrame - this.FadeOutFrame)
                {
                    opacity = Math.Clamp((itemRightFrame - itemFrame) * (1.0 / this.FadeOutFrame), 0, 1);
                }
            }

            return opacity;
        }
        /// <summary>
        /// Called when public properties changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int GetLocalFrame(int i)
        {
            return Convert.ToInt32(i + this.Start.TotalFrames - this.Position.TotalFrames);
        }
    }
}