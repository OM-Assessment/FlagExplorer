using FlagExplorerBackend.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlagExplorerBackend.Converters;
public class CountryJsonConverter : JsonConverter<Country>
{
    public override Country Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        return new Country
        {
            Name = doc.RootElement.GetProperty("name").GetProperty("common").GetString()!,
            Flag = doc.RootElement.GetProperty("flags").GetProperty("png").GetString()!
        };
    }

    public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteString("flagUrl", value.Flag);
        writer.WriteEndObject();
    }
}