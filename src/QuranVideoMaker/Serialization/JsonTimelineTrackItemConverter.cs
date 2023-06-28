using QuranVideoMaker.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Serialization
{
    /// <summary>
    /// Converts an <see cref="ITrackItem"/> object to or from JSON.
    /// </summary>
    public class JsonTimelineTrackItemConverter : JsonConverter<ITrackItem>
    {
        /// <inheritdoc/>
        public override ITrackItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Start object expected.");
            }

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                if (doc.RootElement.TryGetProperty(nameof(ITrackItem.Type), out JsonElement property))
                {
                    var value = string.Empty;

                    //compat
                    if (property.ValueKind == JsonValueKind.Number)
                    {
                        value = ((TrackItemType)property.GetInt32()).ToString();
                    }
                    else
                    {
                        value = property.GetString();
                    }

                    var type = Enum.Parse<TrackItemType>(value);
                    var json = doc.RootElement.GetRawText();

                    switch (type)
                    {
                        case TrackItemType.Quran:
                            return JsonSerializer.Deserialize<QuranTrackItem>(json, options);
                        default:
                            return JsonSerializer.Deserialize<TrackItem>(json, options);
                    }
                }

            }

            return new TrackItem();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ITrackItem value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
