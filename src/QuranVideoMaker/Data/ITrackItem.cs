using System.ComponentModel;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents a track item.
    /// </summary>
    public interface ITrackItem : INotifyPropertyChanged
    {
        /// <summary>
        /// The Id of the track item.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The type of track item.
        /// </summary>
        TrackItemType Type { get; set; }

        /// <summary>
        /// The id of the associated clip.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string ClipId { get; set; }

        /// <summary>
        /// The name of the track item.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the item thumbnail.
        /// </summary>
        string Thumbnail { get; set; }

        /// <summary>
        /// Additional notes about the track item.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        string Note { get; set; }

        /// <summary>
        /// The number of frames to fade in.
        /// </summary>
        double FadeInFrame { get; set; }

        /// <summary>
        /// The number of frames to fade out.
        /// </summary>
        double FadeOutFrame { get; set; }

        /// <summary>
        /// Whether the track item is selected.
        /// </summary>
        [JsonIgnore]
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is changing fade in.
        /// </summary>
        [JsonIgnore]
        bool IsChangingFadeIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is changing fade out.
        /// </summary>
        [JsonIgnore]
        bool IsChangingFadeOut { get; set; }

        /// <summary>
        /// The position of the track item.
        /// </summary>
        TimeCode Position { get; set; }

        /// <summary>
        /// The start time of the track item's associated clip.
        /// </summary>
        TimeCode Start { get; set; }

        /// <summary>
        /// The end time of the track item's associated clip.
        /// </summary>
        TimeCode End { get; set; }

        /// <summary>
        /// The duration of the clip.
        /// </summary>
        [JsonIgnore]
        TimeCode Duration { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item has an unlimited source length.
        /// </summary>
        bool UnlimitedSourceLength { get; set; }

        /// <summary>
        /// Gets the right time of the track item (i.e. position + duration).
        /// </summary>
        TimeCode GetRightTime();

        /// <summary>
        /// Gets the local frame of the track item (i.e. timelineFrame - position).
        /// </summary>
        double GetLocalFrame(double timelineFrame);

        /// <summary>
        /// Gets the timeline frame of the track item (i.e. localFrame + position).
        /// </summary>
        double GetTimelineFrame(double localFrame);

        /// <summary>
        /// Gets the opacity of the track item at the specified local frame.
        /// </summary>
        double GetOpacity(double localFrame);

        /// <summary>
        /// Checks whether the track item is compatible with the specified track type.
        /// </summary>
        bool IsCompatibleWith(TimelineTrackType trackType);

        /// <summary>
        /// Clones this instance.
        /// </summary>
        ITrackItem Clone();

        /// <summary>
        /// Generates a new id for the track item.
        /// </summary>
        /// <returns>The new Id.</returns>
        string GenerateNewId();
    }
}
