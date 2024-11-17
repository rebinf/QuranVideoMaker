namespace QuranVideoMaker.Tests.Data
{
    public class TimelineTrackTests
    {
        [Fact]
        public void CutItem()
        {
            // Arrange
            var track = new TimelineTrack();

            var item = new TrackItem()
            {
                SourceLength = new TimeCode(1500, 25),
                Position = new TimeCode(100, 25), // 4 seconds
                Start = new TimeCode(200, 25), // 8 seconds
                End = new TimeCode(800, 25) // 32 seconds
            };

            var right = item.GetRightTime(); // 32 seconds
            var duration = item.Duration; // 32 seconds

            track.Items.Add(item);

            // Act
            track.CutItem(item, 300); // cut at 12 seconds

            // Assert
            Assert.Equal(2, track.Items.Count); // we should have 2 items
            Assert.Equal(100, track.Items[0].Position.TotalFrames); // first item should start at 4 seconds as it was before
            Assert.Equal(300, track.Items[0].GetRightTime().TotalFrames); // first item should end at 12 seconds
            Assert.Equal(300, track.Items[1].Position.TotalFrames); // second item should start at 12 seconds
            Assert.Equal(right.TotalFrames, track.Items[1].GetRightTime().TotalFrames); // second item should end at 32 seconds as it was before
            Assert.Equal(duration.TotalFrames, track.Items[0].Duration.TotalFrames + track.Items[1].Duration.TotalFrames); // the duration of the two items should be the same as the original item
        }
    }
}
