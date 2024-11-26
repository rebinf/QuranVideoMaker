namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents a recent project with a number, path, and name.
    /// </summary>
    public class RecentProject
    {
        /// <summary>
        /// Gets or sets the number of the recent project.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the path of the recent project.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the recent project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject"/> class.
        /// </summary>
        public RecentProject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject"/> class with the specified path.
        /// </summary>
        /// <param name="path">The path of the recent project.</param>
        public RecentProject(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProject"/> class with the specified number and path.
        /// </summary>
        /// <param name="number">The number of the recent project.</param>
        /// <param name="path">The path of the recent project.</param>
        public RecentProject(int number, string path)
        {
            Number = number;
            Path = path;
            Name = $"{number} - {path}";
        }
    }
}
