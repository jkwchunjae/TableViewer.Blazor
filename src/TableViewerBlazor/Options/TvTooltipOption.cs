using TableViewerBlazor.Options.Property;

namespace TableViewerBlazor.Options;

public class TvTooltipOption
{
    /// <summary>
    /// Sets the text to be displayed inside the tooltip.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Behavior)]
    public string Text { get; set; } = string.Empty;
    public TvTooltipProperty TooltipStyleProperty { get; set; } = new();
}