namespace TableViewerBlazor.Options;

public class TvOptions
{
    public int OpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool ReadProperty { get; set; } = true;
    public bool ReadField { get; set; } = true;
    public IEnumerable<ITvAction>? Actions { get; set; }
    public IEnumerable<TvColumnVisibleOption>? ColumnVisible { get; set; }
    public IEnumerable<string>? DisableKeys { get; set; }
    public TvStyleOption Styles { get; set; } = new();
}

public class TvStyleOption
{
    public bool Dense { get; set; } = true;
    public bool Hover { get; set; } = false;
    public bool Striped { get; set; } = false;
    public bool Bordered { get; set; } = true;
}
