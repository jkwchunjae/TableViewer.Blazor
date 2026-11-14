namespace TableViewer.Share.Options;

/// <summary>Common viewer policies, with the host's native appearance and action types.</summary>
public class TvOptions<TStyle, TButton, TImageOption>
    where TStyle : new()
    where TImageOption : ITvCustomOption
{
    public string? Title { get; set; }
    public int GlobalOpenDepth { get; set; } = 1;
    public int ArrayVisibleDepth { get; set; } = 1;
    public bool VisibleItemCount { get; set; } = true;
    public List<TButton> Buttons { get; set; } = [];
    public List<TButton> TitleButtons { get; set; } = [];
    public List<TvColumnVisibleOption> ColumnVisible { get; set; } = [];
    public List<string> DisableKeys { get; set; } = [];
    public TStyle Style { get; set; } = new();
    public List<ITvEditorOption> EditorOptions { get; set; } = [];
    public List<ITvStringLinkOption> StringLinkOptions { get; set; } = [];
    public List<ITvOpenDepthOption> OpenDepth { get; set; } = [];
    public List<TImageOption> ImageViewerOptions { get; set; } = [];
    public TvDateTimeOption? DateTime { get; set; }
    public TvContents Contents { get; set; } = new();
}

public interface ITvCustomOption
{
    Func<object?, object, int, string, bool> Condition { get; }
}

public static class TvOptionsExtension
{
    public static bool TryGetCustomOption<TStyle, TButton, TImageOption>(this TvOptions<TStyle, TButton, TImageOption> options,
        object? parent, object data, int depth, string path, out ITvCustomOption option)
        where TStyle : new() where TImageOption : ITvCustomOption
    {
        option = options.EditorOptions.Cast<ITvCustomOption>().Concat(options.StringLinkOptions)
            .Concat(options.ImageViewerOptions.Cast<ITvCustomOption>())
            .FirstOrDefault(candidate => candidate.Condition(parent, data, depth, path))!;
        return option is not null;
    }

    public static bool HasEditorOption<TStyle, TButton, TImageOption>(this TvOptions<TStyle, TButton, TImageOption> options,
        object? parent, object data, int depth, string path)
        where TStyle : new() where TImageOption : ITvCustomOption =>
        options.EditorOptions.Any(option => option.Condition(parent, data, depth, path));
}
