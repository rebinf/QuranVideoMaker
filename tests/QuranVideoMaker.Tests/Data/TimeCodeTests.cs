namespace QuranVideoMaker.Tests.Data
{
    public class TimeCodeTests
    {
        [Theory]
        [InlineData(0, 24, 0, 0, 0)] // 0 frames
        [InlineData(24, 24, 0, 0, 1)] // 1 second
        [InlineData(48, 24, 0, 0, 2)] // 2 seconds
        [InlineData(1440, 24, 0, 1, 0)] // 1 minute
        [InlineData(1464, 24, 0, 1, 1)] // 1 minute 1 second
        [InlineData(86400, 24, 1, 0, 0)] // 1 hour
        [InlineData(86424, 24, 1, 0, 1)] // 1 hour 1 second
        [InlineData(1500, 30, 0, 0, 50)] // 50 seconds
        [InlineData(1950, 30, 0, 1, 5)] // 1 minute 5 seconds
        public void Calculate(double frames, double fps, int resultHours, int resultMinutes, int resultSeconds)
        {
            // Arrange

            // Act
            var timeCode = new TimeCode(frames, fps);

            // Assert
            Assert.Equal(frames, timeCode.TotalFrames);
            Assert.Equal(resultHours, timeCode.Hours);
            Assert.Equal(resultMinutes, timeCode.Minutes);
            Assert.Equal(resultSeconds, timeCode.Seconds);
        }

        [Theory]
        [InlineData(0, 24, 0, 0)] // 0 frames
        [InlineData(24, 24, 0, 24)] // 24 frames
        [InlineData(1500, 24, 25, 1525)] // 1525 frames
        public void AddFrames(double totalFrames, double fps, int framesToAdd, double resultTotalFrames)
        {
            // Arrange
            var timeCode = new TimeCode(totalFrames, fps);

            // Act
            var result = timeCode.AddFrames(framesToAdd);

            // Assert
            Assert.Equal(resultTotalFrames, result.TotalFrames);
        }

        [Theory]
        [InlineData(0, 24, 0, 0)] // 0 frames
        [InlineData(24, 24, 1, 48)] // 48 frames
        [InlineData(48, 24, 2, 96)] // 96 frames
        [InlineData(1440, 24, 60, 2880)] // 2880 frames
        [InlineData(1464, 24, 61, 2928)] // 2928 frames
        [InlineData(86400, 24, 3600, 172800)] // 172800 frames
        public void AddSeconds(double totalFrames, double fps, int secondsToAdd, double resultTotalFrames)
        {
            // Arrange
            var timeCode = new TimeCode(totalFrames, fps);

            // Act
            var result = timeCode.AddSeconds(secondsToAdd);

            // Assert
            Assert.Equal(resultTotalFrames, result.TotalFrames);
        }

        [Theory]
        [InlineData(0, 24, 0, 0, 0, 0)] // 0 seconds
        [InlineData(1, 24, 24, 0, 0, 1)] // 1 second
        [InlineData(2, 24, 48, 0, 0, 2)] // 2 seconds
        [InlineData(60, 24, 1440, 0, 1, 0)] // 1 minute
        [InlineData(61, 24, 1464, 0, 1, 1)] // 1 minute 1 second
        [InlineData(3600, 24, 86400, 1, 0, 0)] // 1 hour
        [InlineData(3601, 24, 86424, 1, 0, 1)] // 1 hour 1 second
        [InlineData(50, 30, 1500, 0, 0, 50)] // 50 seconds
        [InlineData(65, 30, 1950, 0, 1, 5)] // 1 minute 5 seconds
        public void FromSeconds(int seconds, double fps, double resultFrames, int resultHours, int resultMinutes, int resultSeconds)
        {
            // Arrange

            // Act
            var timeCode = TimeCode.FromSeconds(seconds, fps);

            // Assert
            Assert.Equal(resultFrames, timeCode.TotalFrames);
            Assert.Equal(resultHours, timeCode.Hours);
            Assert.Equal(resultMinutes, timeCode.Minutes);
            Assert.Equal(resultSeconds, timeCode.Seconds);
        }

        [Theory]
        [InlineData(0, 24, 0, 0, 0, 0)] // 0 seconds
        [InlineData(1, 24, 24, 0, 0, 1)] // 1 second
        [InlineData(2, 24, 48, 0, 0, 2)] // 2 seconds
        [InlineData(60, 24, 1440, 0, 1, 0)] // 1 minute
        [InlineData(61, 24, 1464, 0, 1, 1)] // 1 minute 1 second
        [InlineData(3600, 24, 86400, 1, 0, 0)] // 1 hour
        [InlineData(3601, 24, 86424, 1, 0, 1)] // 1 hour 1 second
        [InlineData(50, 30, 1500, 0, 0, 50)] // 50 seconds
        [InlineData(65, 30, 1950, 0, 1, 5)] // 1 minute 5 seconds
        public void FromTimeSpan(int seconds, double fps, double resultFrames, int resultHours, int resultMinutes, int resultSeconds)
        {
            // Arrange
            var timeSpan = new TimeSpan(0, 0, seconds);

            // Act
            var timeCode = TimeCode.FromTimeSpan(timeSpan, fps);

            // Assert
            Assert.Equal(resultFrames, timeCode.TotalFrames);
            Assert.Equal(resultHours, timeCode.Hours);
            Assert.Equal(resultMinutes, timeCode.Minutes);
            Assert.Equal(resultSeconds, timeCode.Seconds);
        }

        [Theory]
        [InlineData(0, 0, 0, 24, 0)] // 0 frames
        [InlineData(0, 0, 20, 24, 480)] // 480 frames
        [InlineData(0, 1, 0, 24, 1440)] // 1440 frames
        [InlineData(0, 1, 1, 24, 1464)] // 1464 frames
        [InlineData(1, 0, 0, 24, 86400)] // 86400 frames
        public void FromTime(int hours, int minutes, int seconds, double fps, double resultFrames)
        {
            // Arrange

            // Act
            var timeCode = TimeCode.FromTime(hours, minutes, seconds, fps);

            // Assert
            Assert.Equal(resultFrames, timeCode.TotalFrames);
            Assert.Equal(hours, timeCode.Hours);
            Assert.Equal(minutes, timeCode.Minutes);
            Assert.Equal(seconds, timeCode.Seconds);

        }

        [Fact]
        public void Equal()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(24, 24);

            // Act
            var result = timeCode1.Equals(timeCode2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void NotEqual()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(25, 24);

            // Act
            var result = timeCode1.Equals(timeCode2);
        }

        [Fact]
        public void EqualOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(24, 24);

            // Act
            var result = timeCode1 == timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void NotEqualOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(25, 24);

            // Act
            var result = timeCode1 != timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GreaterThanOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(25, 24);
            var timeCode2 = new TimeCode(24, 24);

            // Act
            var result = timeCode1 > timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GreaterThanOrEqualOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(25, 24);
            var timeCode2 = new TimeCode(24, 24);

            // Act
            var result = timeCode1 >= timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void LessThanOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(25, 24);

            // Act
            var result = timeCode1 < timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void LessThanOrEqualOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(25, 24);

            // Act
            var result = timeCode1 <= timeCode2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(24, 24);
            var timeCode2 = new TimeCode(25, 24);

            // Act
            var result = timeCode1 + timeCode2;

            // Assert
            Assert.Equal(49, result.TotalFrames);
        }

        [Fact]
        public void SubtractOperator()
        {
            // Arrange
            var timeCode1 = new TimeCode(25, 24);
            var timeCode2 = new TimeCode(24, 24);

            // Act
            var result = timeCode1 - timeCode2;

            // Assert
            Assert.Equal(1, result.TotalFrames);
        }

        [Fact]
        public void ToStringTest()
        {
            // Arrange
            var timeCode = TimeCode.FromTime(0, 5, 20, 30);

            // Act
            var result = timeCode.ToString();

            // Assert
            Assert.Equal("00:05:20:00", result);
        }

        [Fact]
        public void GetHashCodeTest()
        {
            // Arrange
            var timeCode = new TimeCode(24, 24);

            // Act
            var result = timeCode.GetHashCode();

            // Assert
            Assert.Equal(1078460416, result);
        }
    }
}