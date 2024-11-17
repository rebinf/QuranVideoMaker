using QuranVideoMaker.Serialization;

namespace QuranVideoMaker.Tests.Serialization
{
    public class SerializationTests
    {
        [Fact]
        public void SerializeProjectBasic()
        {
            // Arrange
            var project = TestData.TestProject();

            // Act
            var serialized = ProjectSerializer.Serialize(project);
            var deserialized = ProjectSerializer.Deserialize<Project>(serialized);

            // Assert
            Assert.Equal(project.Chapter, deserialized.Chapter);
            Assert.Equal(project.VerseFrom, deserialized.VerseFrom);
            Assert.Equal(project.VerseTo, deserialized.VerseTo);
            Assert.Equal(project.Clips.Count, deserialized.Clips.Count);
            Assert.Equal(project.Tracks.Count, deserialized.Tracks.Count);
        }
    }
}
