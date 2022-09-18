using QuranVideoMaker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuranVideoMaker.Serialization
{
    public class JsonTrackItemConverter : JsonConverter<TrackItemBase>
    {
        public override TrackItemBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Start object expected.");
            }

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                if (doc.RootElement.TryGetProperty(nameof(TrackItemBase.Type), out JsonElement property))
                {
                    var type = (TrackItemType)property.GetInt32();
                    var json = doc.RootElement.GetRawText();

                    switch (type)
                    {
                        case TrackItemType.Quran:
                            return JsonSerializer.Deserialize<QuranTrackItem>(json);
                        case TrackItemType.Video:
                            return JsonSerializer.Deserialize<TrackItemBase>(json);
                        case TrackItemType.Audio:
                            return JsonSerializer.Deserialize<TrackItemBase>(json);
                        case TrackItemType.Image:
                            return JsonSerializer.Deserialize<TrackItemBase>(json);
                        case TrackItemType.Effect:
                            return JsonSerializer.Deserialize<TrackItemBase>(json);
                        default:
                            break;
                    }
                }
            }

            return new TrackItemBase();
        }

        public override void Write(Utf8JsonWriter writer, TrackItemBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value);
        }
    }
}
