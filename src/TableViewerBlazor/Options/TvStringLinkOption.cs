namespace TableViewerBlazor.Options;

public interface ITvStringLinkOption : ITvCustomOption
{
    Func<object, object, string> Href { get; }
}

public class TvStringLinkOption<TParent, TValue> : ITvStringLinkOption
{
    public Func<TParent, TValue, string> Href { get; set; } = (_, _) => string.Empty;
    public Func<TValue, int, string, bool> Condition { get; set; } = (data, depth, path) => true;
    

    Func<object, object, string> ITvStringLinkOption.Href => (parent, data) => parent is TParent p && data is TValue t ? Href(p,t) : string.Empty;
    Func<object, int, string, bool> ITvCustomOption.Condition => (data, depth, path) => data is TValue t ? Condition.Invoke(t, depth, path) : false;
}