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
    public TvStyleOption Style { get; set; } = new();
    public IEnumerable<ITvEditorOption>? Editor { get; set; }

    public bool TryGetEditorOption(object? data, int depth, string path, out ITvEditorOption option)
    {
        var editorOption = Editor?.FirstOrDefault(x => x.Condition?.Invoke(data, depth, path) ?? false);

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
