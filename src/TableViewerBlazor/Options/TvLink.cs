namespace TableViewerBlazor.Options;

public interface ITvLink
{
    public Func<object?, int, bool> Condition { get; }
    public string Target { get; }
    public Func<object?, string> Href { get; }
    public string Label { get; }
    public TvButtonStyle Style { get; }
}

public class TvLink<T> : ITvLink
{
    public Func<T , int, bool> Condition { get; set; } = (_, _) => true;
    public string Target { get; set; } = "_self";
    public required Func<T, string> Href { get; set; }
    public string Label { get; set; } = "LINK";
    public TvButtonStyle Style { get; set; } = new();

    Func<object?, int, bool> ITvLink.Condition => (o, i) => o is T t && Condition(t, i);
    Func<object?, string> ITvLink.Href => o => o is T t ? Href(t) : string.Empty;
}
