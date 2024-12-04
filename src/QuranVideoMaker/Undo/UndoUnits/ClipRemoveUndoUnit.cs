using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit for removing clips from a project.
    /// </summary>
    public class ClipRemoveUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get; } = "Remove Clip";

        /// <summary>
        /// Gets or sets the project associated with this undo unit.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets the list of clips to be removed.
        /// </summary>
        public List<IProjectClip> Clips { get; } = new List<IProjectClip>();

        /// <summary>
        /// Gets the list of track and item data associated with the clips.
        /// </summary>
        public List<TrackAndItemData> Items { get; } = new List<TrackAndItemData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipRemoveUndoUnit"/> class.
        /// </summary>
        public ClipRemoveUndoUnit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipRemoveUndoUnit"/> class with the specified project.
        /// </summary>
        /// <param name="project">The project associated with this undo unit.</param>
        public ClipRemoveUndoUnit(Project project)
        {
            Project = project;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipRemoveUndoUnit"/> class with the specified project and clip.
        /// </summary>
        /// <param name="project">The project associated with this undo unit.</param>
        /// <param name="clip">The clip to be removed.</param>
        public ClipRemoveUndoUnit(Project project, IProjectClip clip)
        {
            Project = project;
            Clips.Add(clip);
        }

        /// <summary>
        /// Undoes the removal of clips and items.
        /// </summary>
        public void Undo()
        {
            foreach (var clip in Clips)
            {
                Project.Clips.Add(clip);
            }

            foreach (var item in Items)
            {
                item.Track.Items.Add(item.Item);
            }
        }

        /// <summary>
        /// Redoes the removal of clips and items.
        /// </summary>
        public void Redo()
        {
            foreach (var clip in Clips)
            {
                Project.Clips.Remove(clip);
            }

            foreach (var item in Items)
            {
                item.Track.Items.Remove(item.Item);
            }
        }
    }
}
