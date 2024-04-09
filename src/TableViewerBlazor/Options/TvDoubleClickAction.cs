using MudBlazor;

namespace TableViewerBlazor.Options;

public interface ITvDoubleClickAction : ITvAction
{
    public TvSecondButtonStyle SecondStyle { get; }
    public TimeSpan ResetDelay { get; }
}
public class TvDoubleClickAction<T> : TvAction<T>, ITvDoubleClickAction
{
    public TvSecondButtonStyle SecondStyle { get; set; } = new();

    public TimeSpan ResetDelay { get; set; } = TimeSpan.FromSeconds(2);
}

public class TvSecondButtonStyle
{
    public Variant Variant { get; set; } = Variant.Outlined;
    public Color Color { get; set; } = Color.Default;
    public Color IconColor { get; set; } = Color.Default;
}