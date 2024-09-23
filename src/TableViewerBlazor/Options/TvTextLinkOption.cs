namespace TableViewerBlazor.Options;

public class TvTextLinkAttribute : TvFieldAttribute
{
    public TvTextLinkAttribute(string id)
        : base(id)
    {
    }
}

public interface ITvTextLinkOption : ITvFieldOption
{
    public Func<object, string> Href { get; }
    public Func<object, bool> Condition { get; }
}

public class TvTextLinkOption<T> : ITvTextLinkOption where T : class
{
    public string Id { get; init; } = string.Empty;
    Func<object, string> ITvTextLinkOption.Href => o => o is T t ? Href(t) : string.Empty;
    Func<object, bool> ITvTextLinkOption.Condition => o => o is T t ? Condition(t) : true;
    
    public required Func<T, string> Href { get; init; }
    public Func<T, bool> Condition { get; init; } = _ => true;
}
