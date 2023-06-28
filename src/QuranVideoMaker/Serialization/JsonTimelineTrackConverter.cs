using QuranVideoMaker.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Serialization
{
    /// <summary>
    /// Converts an <see cref="ITimelineTrack"/> object to or from JSON.
    /// </summary>
    public class JsonTimelineTrackConverter : JsonConverter<ITimelineTrack>
    {
        /// <inheritdoc/>
        public override ITimelineTrack Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Start object expected.");
            }

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                if (doc.RootElement.TryGetProperty(nameof(ITimelineTrack.Type), out JsonElement property))
                {
                    var type = Enum.Parse<TimelineTrackType>(property.GetString());
                    var json = doc.RootElement.GetRawText();

                    switch (type)
                    {
                        case TimelineTrackType.Quran:
                            return JsonSerializer.Deserialize<TimelineTrack>(json, options);
                        case TimelineTrackType.Video:
                            return JsonSerializer.Deserialize<TimelineTrack>(json, options);
                        case TimelineTrackType.Audio:
                            return JsonSerializer.Deserialize<TimelineTrack>(json, options);
                        default:
                            return JsonSerializer.Deserialize<TimelineTrack>(json, options);
                    }
                }
            }

            return new TimelineTrack();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ITimelineTrack value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
