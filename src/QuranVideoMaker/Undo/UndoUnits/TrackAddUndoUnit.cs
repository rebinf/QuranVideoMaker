using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit for adding a track to the project.
    /// </summary>
    public class TrackAddUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get { return "Track Add"; } }

        /// <summary>
        /// Gets or sets the project associated with this undo unit.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the track associated with this undo unit.
        /// </summary>
        public TimelineTrack Track { get; set; }

        /// <summary>
        /// Gets or sets the index of the track in the project's track collection.
        /// </summary>
        public int TrackIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackAddUndoUnit"/> class.
        /// </summary>
        public TrackAddUndoUnit()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackAddUndoUnit"/> class with the specified project, track, and track index.
        /// </summary>
        /// <param name="project">The project associated with this undo unit.</param>
        /// <param name="track">The track associated with this undo unit.</param>
        /// <param name="trackIndex">The index of the track in the project's track collection.</param>
        public TrackAddUndoUnit(Project project, TimelineTrack track, int trackIndex)
        {
            Project = project;
            Track = track;
            TrackIndex = trackIndex;
        }

        /// <summary>
        /// Undoes the addition of the track to the project.
        /// </summary>
        public void Undo()
        {
            Project.Tracks.Remove(Track);
        }

        /// <summary>
        /// Redoes the addition of the track to the project.
        /// </summary>
        public void Redo()
        {
            Project.Tracks.Insert(TrackIndex, Track);
        }
    }
}
