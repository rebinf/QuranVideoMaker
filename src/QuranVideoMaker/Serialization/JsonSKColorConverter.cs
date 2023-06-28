using SkiaSharp;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuranVideoMaker.Serialization
{
    public class JsonSKColorConverter : JsonConverter<SKColor>
    {
        public override SKColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                return SKColors.Transparent;
            }

            var value = reader.GetString().Split(",");

            if (value.Length == 4)
            {
                return new SKColor(Convert.ToByte(value[1]), Convert.ToByte(value[2]), Convert.ToByte(value[3]), Convert.ToByte(value[0]));
            }

            return SKColors.Transparent;
        }

        public override void Write(Utf8JsonWriter writer, SKColor value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, $"{value.Alpha},{value.Red},{value.Green},{value.Blue}", options);
        }
    }
}
