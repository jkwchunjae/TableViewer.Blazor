namespace TableViewerBlazor.Options.Property;

public class TvTooltipProperty
{
    /// <summary>
    /// If true, the tooltip will be disabled; the popover will not be visible.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; init; }

    /// <summary>
    /// The visible state of the Tooltip.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Visible { get; init; }

    /// <summary>
    /// An event triggered when the state of Visible has changed
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public EventCallback<bool> VisibleChanged { get; init; }

    /// <summary>
    /// If true, an arrow will be displayed pointing towards the content from the tooltip.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public bool Arrow { get; init; }

    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public Color Color { get; init; } = Color.Default;

    /// <summary>
    /// Sets the amount of time in milliseconds to wait from opening the popover before beginning to perform the transition. 
    /// </summary>
    /// <remarks>
    /// Set globally via <see cref="MudGlobal.TooltipDefaults.Delay"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public double Delay { get; init; } = MudGlobal.TooltipDefaults.Delay.TotalMilliseconds;

    /// <summary>
    /// Sets the length of time that the opening transition takes to complete.
    /// </summary>
    /// <remarks>
    /// Set globally via <see cref="MudGlobal.TransitionDefaults.Duration"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public double Duration { get; init; } = MudGlobal.TransitionDefaults.Duration.TotalMilliseconds;

    /// <summary>
    /// Determines if this component should be inline with it's surrounding (default) or if it should behave like a block element.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public bool Inline { get; init; }

    /// <summary>
    /// Tooltip placement.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public Placement Placement { get; init; } = Placement.Bottom;

    /// <summary>
    /// Classes applied directly to root component of the tooltip
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public string RootClass { get; init; } = string.Empty;

    /// <summary>
    /// Styles applied directly to root component of the tooltip
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public string RootStyle { get; init; } = string.Empty;

    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public bool ShowOnClick { get; init; }

    /// <summary>
    /// Determines on which events the tooltip will act
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public bool ShowOnFocus { get; init; }

    /// <summary>
    /// Determines on which events the tooltip will act
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Tooltip.Appearance)]
    public bool ShowOnHover { get; init; } = true;
}