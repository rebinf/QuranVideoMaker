namespace QuranVideoMaker.Tests.Data
{
    public class TrackItemTests
    {
        [Theory]
        [InlineData(-5, 0)]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(1500, 749)]
        [InlineData(1505, 749)]
        [InlineData(2000, 749)]
        public void SetStart(double startFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.Start = new TimeCode(startFrame, 25);

            // Assert
            Assert.Equal(resultFrames, trackItem.Start.TotalFrames);
        }

        [Theory]
        [InlineData(-5, 0)]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(1500, 749)]
        [InlineData(1505, 749)]
        [InlineData(2000, 749)]
        public void SetStartUnlimited(double startFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                UnlimitedSourceLength = true,
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.Start = new TimeCode(startFrame, 25);

            // Assert
            Assert.Equal(resultFrames, trackItem.Start.TotalFrames);
        }

        [Theory]
        [InlineData(-5, 1)]
        [InlineData(0, 1)]
        [InlineData(5, 5)]
        [InlineData(1500, 1500)]
        [InlineData(1505, 1500)]
        [InlineData(2000, 1500)]
        public void SetEnd(double endFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.End = new TimeCode(endFrame, 25);

            // Assert
            Assert.Equal(resultFrames, trackItem.End.TotalFrames);
        }

        [Theory]
        [InlineData(-5, 1)]
        [InlineData(0, 1)]
        [InlineData(5, 5)]
        [InlineData(1500, 1500)]
        [InlineData(1505, 1505)]
        [InlineData(2000, 2000)]
        public void SetEndUnlimited(double endFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                UnlimitedSourceLength = true,
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.End = new TimeCode(endFrame, 25);

            // Assert
            Assert.Equal(resultFrames, trackItem.End.TotalFrames);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(750, 725)]
        [InlineData(1000, 725)]
        public void SetFadeInFrame(double fadeFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.FadeInFrame = fadeFrame;

            // Assert
            Assert.Equal(resultFrames, trackItem.FadeInFrame);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(750, 725)]
        [InlineData(1000, 725)]
        public void SetFadeOutFrame(double fadeFrame, double resultFrames)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25)
            };

            // Act
            trackItem.FadeOutFrame = fadeFrame;

            // Assert
            Assert.Equal(resultFrames, trackItem.FadeOutFrame);
        }

        [Theory]
        [InlineData(0, 0, 100, 100)]
        [InlineData(100, 0, 200, 100)]
        [InlineData(0, 100, 100, 200)]
        [InlineData(100, 100, 200, 200)]
        public void GetLocalFrame(double position, double start, double timelineFrame, double localFrame)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25),
                Start = new TimeCode(start, 25),
                Position = new TimeCode(position, 25)
            };

            // Act
            var result = trackItem.GetLocalFrame(timelineFrame);

            // Assert
            Assert.Equal(localFrame, result);
        }

        [Theory]
        [InlineData(0, 100, 100)]
        [InlineData(300, 350, 50)]
        public void GetTimelineFrame(double position, double timelineFrame, double localFrame)
        {
            // Arrange
            var trackItem = new TrackItem()
            {
                SourceDuration = TimeCode.FromSeconds(60, 25),
                End = TimeCode.FromSeconds(30, 25),
                Position = new TimeCode(position, 25)
            };

            // Act
            var result = trackItem.GetTimelineFrame(localFrame);

            // Assert
            Assert.Equal(timelineFrame, result);
        }
    }
}
