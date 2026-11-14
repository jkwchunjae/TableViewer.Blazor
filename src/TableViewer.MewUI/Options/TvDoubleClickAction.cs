namespace TableViewer.MewUI.Options;

public interface ITvDoubleClickAction : ITvAction, global::TableViewer.Share.Options.ITvDoubleClickAction<TvSecondButtonStyle>
{
}

public class TvDoubleClickAction<T> : TvAction<T>, ITvDoubleClickAction
{
    private readonly global::TableViewer.Share.Options.TvDoubleClickOptions<TvSecondButtonStyle> _options = new();
    public TvSecondButtonStyle SecondStyle { get => _options.SecondStyle; set => _options.SecondStyle = value; }
    public TimeSpan ResetDelay { get => _options.ResetDelay; set => _options.ResetDelay = value; }
}

public class TvSecondButtonStyle() : global::TableViewer.Share.Options.TvSecondButtonStyle<Variant, Color>(Variant.Outlined, Color.Default)
{
}
