namespace TableViewerBlazor.Options;

public class TvOptions
{
    public string? Title { get; set; }
    public int GlobalOpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool VisibleItemCount { get; set; } = true;
    public List<ITvButton> Buttons { get; set; } = [];
    public List<ITvButton> TitleButtons { get; set; } = [];
    public List<TvColumnVisibleOption> ColumnVisible { get; set; } = [];
    public List<string> DisableKeys { get; set; } = [];
    public TvStyleOption Style { get; set; } = new();
    public List<ITvEditorOption> EditorOptions { get; set; } = [];
    public List<ITvStringLinkOption> StringLinkOptions { get; set; } = [];
    public List<ITvOpenDepthOption> OpenDepth { get; set; } = [];
    public TvDateTimeOption? DateTime { get; set; }
    public TvContents Contents { get; set; } = new();
}

public interface ITvCustomOption
{
    Func<object , int, string, bool> Condition { get; }
}

public static class TvOptionsExtension
{
    public static bool TryGetCustomOption(this TvOptions options, object data, int depth, string path, out ITvCustomOption option)
    {
        var customOptions = Enumerable.Empty<ITvCustomOption>()
            .Concat(options.EditorOptions)
            .Concat(options.StringLinkOptions)
            .ToArray();

        var customOption = customOptions.FirstOrDefault(option => option.Condition.Invoke(data, depth, path));
        
        if (customOption != null)
        {
            option = customOption;
            return true;
        }
        else
        {
            option = null!;
            return false;
        }
    }
}