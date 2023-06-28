namespace QuranVideoMaker.Tests
{
    public static class TestData
    {
        public static Project TestProject()
        {
            var project = new Project
            {
                Chapter = 1,
                VerseFrom = 1,
                VerseTo = 7
            };

            var clip1 = new ProjectClip
            {
                FilePath = "test.mp4",
                ItemType = TrackItemType.Video,
            };

            var track1 = new TimelineTrack
            {
                Name = "Track 1",
                Type = TimelineTrackType.Video,
                Height = 100
            };

            var trackItem1 = new TrackItem
            {
                Type = TrackItemType.Quran,
            };

            var trackItem2 = new TrackItem
            {
                ClipId = clip1.Id,
                Type = TrackItemType.Video,
            };

            project.Clips.Add(clip1);
            project.Tracks.Add(track1);
            track1.Items.Add(trackItem1);
            track1.Items.Add(trackItem2);

            return project;
        }
    }
}
