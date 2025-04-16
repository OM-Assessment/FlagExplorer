using FlagExplorerBackend.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlagExplorerBackend.Converters;
public class CountryDetailsJsonConverter : JsonConverter<CountryDetailsDto>
{
    public override CountryDetailsDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        return new CountryDetailsDto
        {
            Name = doc.RootElement.GetProperty("name").GetProperty("common").GetString()!,
            Population = (int)doc.RootElement.GetProperty("population").GetInt64(),
            Capital = doc.RootElement.GetProperty("capital")[0].GetString()!,
            Flag = doc.RootElement.GetProperty("flags").GetProperty("png").GetString()!
        };
    }

    public override void Write(Utf8JsonWriter writer, CountryDetailsDto value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteNumber("population", value.Population);
        writer.WriteString("capital", value.Capital);
        writer.WriteString("flagUrl", value.Flag);
        writer.WriteEndObject();
    }
}