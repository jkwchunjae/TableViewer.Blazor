namespace TableViewer.Share.Options;

public interface ITvImageViewerOption<TFit, TPosition> : ITvCustomOption
{
    Func<object, object, string> ImageUrl { get; }
    Func<object, object, string> FallbackUrl { get; }
    string? Alt { get; }
    bool Fluid { get; }
    int? Height { get; set; }
    int? Width { get; }
    int Elevation { get; }
    TFit ObjectFit { get; }
    TPosition ObjectPosition { get; }
}

/// <summary>Shares image selection and sizing while letting each UI supply its fit and alignment values.</summary>
public class TvImageViewerOption<TParent, TValue, TFit, TPosition>(TFit defaultFit, TPosition defaultPosition)
    : ITvImageViewerOption<TFit, TPosition>
{
    public Func<TParent, TValue, string> ImageUrl { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, string> FallbackUrl { get; set; } = (_, _) => string.Empty;
    public Func<TParent, TValue, int, string, bool> Condition { get; set; } = (_, _, _, _) => true;
    public bool Fluid { get; set; }
    public string? Alt { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public int Elevation { get; set; }
    public TFit ObjectFit { get; set; } = defaultFit;
    public TPosition ObjectPosition { get; set; } = defaultPosition;

    Func<object, object, string> ITvImageViewerOption<TFit, TPosition>.ImageUrl =>
        (parent, data) => parent is TParent p && data is TValue value ? ImageUrl(p, value) : string.Empty;
    Func<object, object, string> ITvImageViewerOption<TFit, TPosition>.FallbackUrl =>
        (parent, data) => parent is TParent p && data is TValue value ? FallbackUrl(p, value) : string.Empty;
    Func<object?, object, int, string, bool> ITvCustomOption.Condition =>
        (parent, data, depth, path) => parent is TParent p && data is TValue value && Condition(p, value, depth, path);
}
