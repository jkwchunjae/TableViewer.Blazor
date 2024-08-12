namespace TableViewerBlazor.Options;

public class TvStyleOption
{
    public bool UseTable { get; set; } = true;
    public bool Dense { get; set; } = true;
    public bool SuperDense { get; set; } = true;
    public bool Hover { get; set; } = false;
    public bool Striped { get; set; } = false;
    public bool Bordered { get; set; } = true;
    public List<string> RootClass { get; set; } = [];
    public Color LoadingProgressColor { get; set; } = Color.Default;
    public bool VisibleInnerToolbar { get; set; } = true;
    public TvClassAndStyle TitleStyle { get; set; } = new TvClassAndStyle
    {
        Class = { "mb-2" },
        Style = { "font-weight: bold;" },
    };
    public TvClassAndStyle ThStyle { get; set; } = new TvClassAndStyle
    {
        Style =
        {
            "font-weight: bold;",
            "background-color: #ccc;",
        },
    };
}

public class TvClassAndStyle
{
    public List<string> Class { get; set; } = [];
    public List<string> Style { get; set; } = [];

    public string ClassStr => $" {string.Join(" ", Class)} ";
    public string StyleStr => $" {string.Join(" ", Style)} ";
}

