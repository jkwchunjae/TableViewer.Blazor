using System.Text.Json;

namespace TableViewerBlazor.Options;

public interface ITvEditorOption
{
    delegate bool EditorOptionCondition(object? data, int depth, string path);
    string Language { get; }
    Func<object, string>? Serializer { get; }
    EditorOptionCondition? Condition { get; }
}

public class TvEditorOption<T> : ITvEditorOption
{
    public delegate bool EditorOptionConditionT(T? data, int depth, string path);
    public string Language { get; set; } = "json";
    public Func<T, string>? Serializer { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) => data is T t && Condition != null ? Condition(t, depth, path) : false;
}

public class TvJsonEditorOption<T> : ITvEditorOption
{
    private static readonly JsonSerializerOptions defaultSerializerOption = new JsonSerializerOptions
    {
        WriteIndented = true,
    };
    public string Language { get; } = "json";
    public Func<T, string> Serializer { get; set; } =
        (T data) => JsonSerializer.Serialize(data, defaultSerializerOption);
    public Func<T?, int, string, bool> Condition { get; set; } =
        (T? data, int depth, string path) => true;

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) => data is T t && Condition != null ? Condition(t, depth, path) : false;
}

public class TvYamlEditorOption<T> : ITvEditorOption
{
    private static readonly YamlDotNet.Serialization.ISerializer defaultSerializer = new YamlDotNet.Serialization.SerializerBuilder()
        .Build();

    public string Language { get; } = "json";
    public Func<T, string> Serializer { get; set; } =
        (T data) => defaultSerializer.Serialize(data);
    public Func<T?, int, string, bool> Condition { get; set; } =
        (T? data, int depth, string path) => true;

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) => data is T t && Condition != null ? Condition(t, depth, path) : false;
}