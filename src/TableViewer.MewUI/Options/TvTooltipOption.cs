using TableViewer.MewUI.Options.Property;

namespace TableViewer.MewUI.Options;

public class TvTooltipOption
{
    /// <summary>
    /// Sets the text to be displayed inside the tooltip.
    /// </summary>

    public string Text { get; set; } = string.Empty;
    public TvTooltipProperty TooltipStyleProperty { get; set; } = new();
}
