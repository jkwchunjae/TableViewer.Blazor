namespace TableViewerBlazor.Options;

public interface ITvDoubleClickAction : ITvAction
{
    public TvButtonStyle InitStyle { get; }
    public TimeSpan ResetDelay { get; }
}
public class TvDoubleClickAction<T> : TvAction<T>, ITvDoubleClickAction
{
    public TvButtonStyle InitStyle { get; set; } = new();

    public TimeSpan ResetDelay { get; set; } = TimeSpan.FromSeconds(2);
}
