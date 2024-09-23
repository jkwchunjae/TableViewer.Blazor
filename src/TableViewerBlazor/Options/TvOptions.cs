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
    public List<TvColumnVisibleOption> ColumnVisible { get; set; } = [];
    public List<string> DisableKeys { get; set; } = [];
    public TvStyleOption Style { get; set; } = new();
    public List<ITvEditorOption> Editor { get; set; } = [];
    public List<ITvOpenDepthOption> OpenDepth { get; set; } = [];
    public List<ITvTextLinkOption> TextLinkOptions { get; set; } = [];
    public TvDateTimeOption? DateTime { get; set; }
    public TvContents Contents { get; set; } = new();

}

public static class TvOptionsExtension
{
    public static bool TryGetFieldOption(this TvOptions options, MemberInfo? memberInfo, out ITvFieldOption? fieldOption)
    {
        var fieldOptions = Enumerable.Empty<ITvFieldOption>()
            .Concat(options.TextLinkOptions)
            .ToArray();

        var fieldAttribute = memberInfo?.GetCustomAttribute<TvFieldAttribute>();
        if (fieldAttribute != default)
        {
            var option = fieldOptions
                .Where(o => !string.IsNullOrEmpty(o.Id))
                .FirstOrDefault(o => o.Id == fieldAttribute.Id);
            if (option != null)
            {
                fieldOption = option;
                return true;
            }
        }

        fieldOption = default;
        return false;
    }
}