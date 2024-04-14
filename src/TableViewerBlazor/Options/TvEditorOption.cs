namespace TableViewerBlazor.Options;

public class TvEditorOption
{
    public string Language { get; set; } = "json";
    public Func<object, string>? Serializer { get; set; }
}
