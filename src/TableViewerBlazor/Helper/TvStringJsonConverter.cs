using System.Text.Json;
using System.Text.Json.Serialization;

namespace TableViewerBlazor.Helper;

public class TvStringJsonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString() ?? string.Empty);
    }
}