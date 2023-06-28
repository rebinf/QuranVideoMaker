using QuranVideoMaker.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Serialization
{
    /// <summary>
    /// Converts an <see cref="IProjectClip"/> object to or from JSON.
    /// </summary>
    public class JsonProjectClipConverter : JsonConverter<IProjectClip>
    {
        /// <inheritdoc/>
        public override IProjectClip Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Start object expected.");
            }

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                if (doc.RootElement.TryGetProperty(nameof(IProjectClip.ItemType), out JsonElement property))
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

                    return JsonSerializer.Deserialize<ProjectClip>(json, options);
                }
            }

            return new ProjectClip();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IProjectClip value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}
