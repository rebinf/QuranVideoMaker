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

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, ProjectSerializerOptions);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, ProjectSerializerOptions);
        }
    }
}
