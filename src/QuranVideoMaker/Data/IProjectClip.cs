using System.ComponentModel;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents a single clip in a project
    /// </summary>
    public interface IProjectClip : INotifyPropertyChanged
    {
        /// <summary>
        /// Unique ID of this clip
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The type of clip this is in the project
        /// </summary>
        TrackItemType ItemType { get; set; }

        /// <summary>
        /// Absolute file path
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// The absolute file path for the thumbnail of this clip
        /// </summary>
        string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        TimeCode Length { get; set; }

        /// <summary>
        /// Gets a value indicating whether this clip has an unlimited length.
        /// </summary>
        bool UnlimitedLength { get; }

        /// <summary>
        /// Name of the file
        /// </summary>
        [JsonIgnore]
        string FileName { get; }

        /// <summary>
        /// Determines if this clip is currently selected
        /// </summary>
        [JsonIgnore]
        bool IsSelected { get; set; }

        [JsonIgnore]
        string TempFramesCacheFile { get; }

        [JsonIgnore]
        List<FrameCache> FramesCache { get; }

        bool IsCompatibleWith(TimelineTrackType trackType);

        void CacheFrames();

        /// <summary>
        /// Generates a new Id for the clip.
        /// </summary>
        /// <returns>The new Id.</returns>
        string GenerateNewId();

    }
}