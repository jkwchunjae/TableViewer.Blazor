namespace TableViewerBlazor.Options;

public class TvOptions
{
    public string? Title { get; set; }
    public int GlobalOpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool VisibleItemCount { get; set; } = true;
    public List<ITvAction> Actions { get; set; } = [];
    public List<ITvButton> TitleButtons { get; set; } = [];
    public List<ITvLink> Links { get; set; } = [];
    public TvStyleOption Style { get; set; } = new();
    public List<ITvEditorOption> Editor { get; set; } = [];
    public List<ITvOpenDepthOption> OpenDepth { get; set; } = [];
    public TvDateTimeOption? DateTime { get; set; }
    public TvContents Contents { get; set; } = new();
}