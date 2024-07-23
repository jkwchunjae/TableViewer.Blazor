namespace TableViewerBlazor.Options;

public class TvOptions
{
    public string? Title { get; set; }
    public int GlobalOpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public List<ITvAction> Actions { get; set; } = [];
    public List<TvColumnVisibleOption> ColumnVisible { get; set; } = [];
    public List<string> DisableKeys { get; set; } = [];
    public TvStyleOption Style { get; set; } = new();
    public List<ITvEditorOption> Editor { get; set; } = [];
    public List<ITvOpenDepthOption> OpenDepth { get; set; } = [];
    public TvDateTimeOption? DateTime { get; set; }
    public List<Type> StringTypes { get; set; } = [];
    public TvContents Contents { get; set; } = new();
}
