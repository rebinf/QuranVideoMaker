using QuranVideoMaker.Data;

namespace QuranVideoMaker.Undo.UndoUnits
{
    /// <summary>
    /// Represents an undo unit for adding clips to a project.
    /// </summary>
    public class ClipAddUndoUnit : IUndoUnit
    {
        /// <summary>
        /// Gets the name of the undo unit.
        /// </summary>
        public string Name { get; } = "Add Clip";

        /// <summary>
        /// Gets or sets the project associated with this undo unit.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets the list of clips added in this undo unit.
        /// </summary>
        public List<IProjectClip> Clips { get; } = new List<IProjectClip>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipAddUndoUnit"/> class.
        /// </summary>
        public ClipAddUndoUnit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipAddUndoUnit"/> class with the specified project.
        /// </summary>
        /// <param name="project">The project associated with this undo unit.</param>
        public ClipAddUndoUnit(Project project)
        {
            Project = project;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipAddUndoUnit"/> class with the specified project and clip.
        /// </summary>
        /// <param name="project">The project associated with this undo unit.</param>
        /// <param name="clip">The clip to be added in this undo unit.</param>
        public ClipAddUndoUnit(Project project, IProjectClip clip)
        {
            Project = project;
            Clips.Add(clip);
        }

        /// <summary>
        /// Undoes the action of adding clips to the project.
        /// </summary>
        public void Undo()
        {
            foreach (var clip in Clips)
            {
                Project.Clips.Remove(clip);
            }
        }

        /// <summary>
        /// Redoes the action of adding clips to the project.
        /// </summary>
        public void Redo()
        {
            foreach (var clip in Clips)
            {
                Project.Clips.Add(clip);
            }
        }
    }
}
