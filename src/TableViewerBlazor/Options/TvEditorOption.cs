using System.Text.Json;

namespace TableViewerBlazor.Options;

public static class TvEditorOptionExtension
{
    public static bool TryGetEditorOption(this TvOptions tvOptions, object? data, int depth, string path, out ITvEditorOption option)
    {
        var editorOption = tvOptions.Editor?.FirstOrDefault(x => x.Condition?.Invoke(data, depth, path) ?? false);

        if (editorOption != null)
        {
            option = editorOption;
            return true;
        }
        else
        {
            option = null!;
            return false;
        }
    }
}

public interface ITvEditorOption
{
    delegate bool EditorOptionCondition(object? data, int depth, string path);
    delegate bool EditorOptionConditionByType(Type type, int depth, string path);
    string Language { get; }
    EditorSize? LayoutMaxSize { get; }
    Func<object, string>? Serializer { get; }
    EditorOptionCondition? Condition { get; }
    EditorOptionConditionByType ConditionByType { get; }
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

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) =>
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

    public ITvEditorOption.EditorOptionConditionByType ConditionByType =>
        (type, depth, path) =>
        {
            if (typeof(T) == type)
            {
                return Condition != null;
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

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) =>
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

    public ITvEditorOption.EditorOptionConditionByType ConditionByType =>
        (type, depth, path) =>
        {
            if (typeof(T) == type)
            {
                return Condition != null;
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

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) =>
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

    public ITvEditorOption.EditorOptionConditionByType ConditionByType =>
        (type, depth, path) =>
        {
            if (typeof(T) == type)
            {
                return Condition != null;
            }
            else
            {
                return false;
            }
        };
}