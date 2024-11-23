namespace QuranVideoMaker.Data
{
    public class RecentProject
    {
        public int Number { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public RecentProject()
        {

        }

        public RecentProject(string path)
        {
            Path = path;
        }

        public RecentProject(int number, string path)
        {
            Number = number;
            Path = path;
            Name = $"{number} - {path}";
        }
    }
}
