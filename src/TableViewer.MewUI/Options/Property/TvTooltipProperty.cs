namespace TableViewer.MewUI.Options.Property;

public class TvTooltipProperty
{
    /// <summary>
    /// If true, the tooltip will be disabled; the popover will not be visible.
    /// </summary>

    public bool Disabled { get; init; }

    /// <summary>
    /// The visible state of the Tooltip.
    /// </summary>

    public bool Visible { get; init; }

    /// <summary>
    /// An event triggered when the state of Visible has changed
    /// </summary>

    public Action<bool>? VisibleChanged { get; init; }

    /// <summary>
    /// If true, an arrow will be displayed pointing towards the content from the tooltip.
    /// </summary>

    public bool Arrow { get; init; }

    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>

    public Color Color { get; init; } = Color.Default;

    /// <summary>
    /// Sets the amount of time in milliseconds to wait from opening the popover before beginning to perform the transition. 
    /// </summary>
    /// <remarks>
    /// Set globally via <see cref="MudGlobal.TooltipDefaults.Delay"/>.
    /// </remarks>

    public double Delay { get; init; } = 500;

    /// <summary>
    /// Sets the length of time that the opening transition takes to complete.
    /// </summary>
    /// <remarks>
    /// Set globally via <see cref="MudGlobal.TooltipDefaults.Duration"/>.
    /// </remarks>

    public double Duration { get; init; } = 100;

    /// <summary>
    /// Determines if this component should be inline with it's surrounding (default) or if it should behave like a block element.
    /// </summary>

    public bool Inline { get; init; }

    /// <summary>
    /// Tooltip placement.
    /// </summary>

    public Placement Placement { get; init; } = Placement.Bottom;

    /// <summary>
    /// Classes applied directly to root component of the tooltip
    /// </summary>

    public string RootClass { get; init; } = string.Empty;

    /// <summary>
    /// Styles applied directly to root component of the tooltip
    /// </summary>

    public string RootStyle { get; init; } = string.Empty;

    public bool ShowOnClick { get; init; }

    /// <summary>
    /// Shows the tooltip on keyboard focus. Click, touch, and programmatic focus do not open it.
    /// </summary>

    public bool ShowOnFocus { get; init; }

    /// <summary>
    /// Determines on which events the tooltip will act
    /// </summary>

    public bool ShowOnHover { get; init; } = true;
}
