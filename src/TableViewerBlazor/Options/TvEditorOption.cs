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

    ITvEditorOption.EditorOptionCondition? ITvEditorOption.Condition =>
        (data, depth, path) => data is T t && Condition != null ? Condition(t, depth, path) : false;

    Func<object, string>? ITvEditorOption.Serializer =>
        (data) => data is T t && Serializer != null ? Serializer.Invoke(t) : string.Empty;
}
