using System.Text.Json;

namespace TableViewerBlazor.Options;

public static class TvEditorOptionExtension
{
    public static bool HasEditorOption(this TvOptions tvOptions, object? parent, object data, int depth, string path)
    {
        return tvOptions.EditorOptions?.FirstOrDefault(e => e.Condition?.Invoke(parent, data, depth, path) ?? false) != null;
    }
}

public interface ITvEditorOption : ITvCustomOption
{
    string Language { get; }
    EditorSize? LayoutMaxSize { get; }
    Func<object, string>? Serializer { get; }
}

public record EditorSize(int? Height, int? Width);

public class TvEditorOption<T> : ITvEditorOption
{
    public delegate bool EditorOptionConditionT(T? data, int depth, string path);
    public string Language { get; set; } = "json";
    public EditorSize? LayoutMaxSize { get; set; }
    public Func<T, string>? Serializer { get; set; }
    public Func<T?, int, string, bool>? Condition { get; set; }

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    Func<object?, object, int, string, bool> ITvCustomOption.Condition =>
        (parent, data, depth, path) =>
        {
            if (data is T t)
            {
                return Condition?.Invoke(t, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}

public class TvJsonEditorOption<T> : ITvEditorOption
{
    private static readonly JsonSerializerOptions defaultSerializerOption = new JsonSerializerOptions
    {
        WriteIndented = true,
    };
    public string Language { get; } = "json";
    public EditorSize? LayoutMaxSize { get; set; }
    public Func<T, string> Serializer { get; set; } =
        (T data) => JsonSerializer.Serialize(data, defaultSerializerOption);
    public Func<T?, int, string, bool> Condition { get; set; } =
        (T? data, int depth, string path) => true;

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    Func<object?, object, int, string, bool> ITvCustomOption.Condition =>
        (parent, data, depth, path) =>
        {
            if (data is T t)
            {
                return Condition?.Invoke(t, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}

public class TvYamlEditorOption<T> : ITvEditorOption
{
    private static readonly YamlDotNet.Serialization.ISerializer defaultSerializer = new YamlDotNet.Serialization.SerializerBuilder()
        .WithTypeConverter(new TvStringYamlConverter())
        .Build();

    public string Language { get; } = "json";
    public EditorSize? LayoutMaxSize { get; set; }
    public Func<T, string> Serializer { get; set; } =
        (T data) => defaultSerializer.Serialize(data);
    public Func<T?, int, string, bool> Condition { get; set; } =
        (T? data, int depth, string path) => true;

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;

    Func<object?, object, int, string, bool> ITvCustomOption.Condition =>
        (parent, data, depth, path) =>
        {
            if (data is T t)
            {
                return Condition?.Invoke(t, depth, path) ?? true;
            }
            else
            {
                return false;
            }
        };
}