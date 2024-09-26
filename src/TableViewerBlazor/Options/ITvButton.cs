namespace TableViewerBlazor.Options;

public interface ITvButton
{
    public Func<object?, int, bool> Condition { get; }
    public string Label { get; }
    public TvButtonStyle Style { get; }
    public TvTooltipOption? TooltipOption { get; }
}