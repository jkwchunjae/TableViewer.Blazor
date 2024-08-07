namespace TableViewerBlazor.Options;

public class TvStyleOption
{
    public bool Dense { get; set; } = true;
    public bool SuperDense { get; set; } = true;
    public bool Hover { get; set; } = false;
    public bool Striped { get; set; } = false;
    public bool Bordered { get; set; } = true;
    public List<string> RootClass { get; set; } = [];
    public Color LoadingProgressColor { get; set; } = Color.Default;
    public bool VisibleInnerToolbar { get; set; } = true;
}
