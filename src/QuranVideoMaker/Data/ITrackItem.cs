using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    public interface ITrackItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        TrackItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the clip identifier.
        /// </summary>
        /// <value>
        /// The clip identifier.
        /// </value>
        string ClipId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        string Note { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this item is selected; otherwise, <c>false</c>.
        /// </value>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is changing fade in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is changing fade in; otherwise, <c>false</c>.
        /// </value>
        bool IsChangingFadeIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is changing fade out.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is changing fade out; otherwise, <c>false</c>.
        /// </value>
        bool IsChangingFadeOut { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        TimeCode Position { get; set; }

        /// <summary>
        /// Gets or sets the length of the source.
        /// </summary>
        /// <value>
        /// The length of the source.
        /// </value>
        TimeCode SourceLength { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        TimeCode Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        TimeCode End { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item has an unlimited source length.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this item has an unlimited source length; otherwise, <c>false</c>.
        /// </value>
        bool UnlimitedSourceLength { get; set; }

        /// <summary>
        /// Gets or sets the fade in frame.
        /// </summary>
        /// <value>
        /// The fade in frame.
        /// </value>
        double FadeInFrame { get; set; }

        /// <summary>
        /// Gets or sets the fade out frame.
        /// </summary>
        /// <value>
        /// The fade out frame.
        /// </value>
        double FadeOutFrame { get; set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        /// <returns></returns>
        double GetWidth(int zoom);

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        /// <returns></returns>
        double GetPosition(int zoom);

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <returns></returns>
        TimeCode GetLength();

        /// <summary>
        /// Gets the fade in position.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        /// <returns></returns>
        double GetFadeInPosition(int zoom);

        /// <summary>
        /// Gets the fade out position.
        /// </summary>
        /// <param name="zoom">The zoom.</param>
        /// <returns></returns>
        double GetFadeOutPosition(int zoom);
    }
}
