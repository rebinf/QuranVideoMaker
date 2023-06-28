using QuranVideoMaker.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Serialization
{
    /// <summary>
    /// The ProjectSerializer class used for serializing and deserializing Project objects.
    /// </summary>
    public static class ProjectSerializer
    {
        /// <summary>
        /// Options for project serialization.
        /// </summary>
        public static readonly JsonSerializerOptions ProjectSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        static ProjectSerializer()
        {
            ProjectSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            ProjectSerializerOptions.Converters.Add(new JsonProjectClipConverter());
            ProjectSerializerOptions.Converters.Add(new JsonTimelineTrackConverter());
            ProjectSerializerOptions.Converters.Add(new JsonTimelineTrackItemConverter());
            ProjectSerializerOptions.Converters.Add(new JsonSKColorConverter());
        }

        /// <summary>
        /// Serializes project.
        /// </summary>
        /// <param name="project">Project to serialize.</param>
        /// <returns>Serialized project string.</returns>
        public static string Serialize(Project project)
        {
            return JsonSerializer.Serialize(project, ProjectSerializerOptions);
        }

        /// <summary>
        /// Deserializes project.
        /// </summary>
        /// <param name="json">Serialized project string.</param>
        /// <returns>Deserialized project.</returns>
        public static Project Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Project>(json, ProjectSerializerOptions);
        }
    }
}
