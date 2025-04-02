namespace TableViewerBlazor.Options;

public interface ITvStringLinkOption : ITvCustomOption
{
    Func<object, object, string> Href { get; }
}

public class TvStringLinkOption<TParent, TValue> : ITvStringLinkOption
{
    public Func<TParent, TValue, string> Href { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, int, string, bool> Condition { get; set; } = (_, _, _, _) => true;
    

    Func<object, object, string> ITvStringLinkOption.Href => (parent, data) => parent is TParent p && data is TValue t ? Href(p,t) : string.Empty;
    Func<object?, object, int, string, bool> ITvCustomOption.Condition => (parent, data, depth, path) => parent is TParent p && data is TValue t ? Condition.Invoke(p, t, depth, path) : false;
}