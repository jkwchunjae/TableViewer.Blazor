namespace TableViewerBlazor.Options.Property;

public class TeRadioItemProperty : TeComponentBaseProperty
{
    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// The base color of the component in its none active/unchecked state. It supports the theme colors.
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public Color? UncheckedColor { get; set; } = null;

    /// <summary>
    /// The position of the child content.
    /// </summary>
    [Category(CategoryTypes.Radio.Behavior)]
    public Placement Placement { get; set; } = Placement.End;

    /// <summary>
    /// If true, compact padding will be applied.
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public bool Dense { get; set; }

    /// <summary>
    /// The Size of the component.
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// Gets or sets whether to show a ripple effect when the user clicks the button. Default is true.
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public bool Ripple { get; set; } = true;

    /// <summary>
    /// If true, the button will be disabled.
    /// </summary>
    [Category(CategoryTypes.Radio.Behavior)]
    public bool Disabled { get; set; }
}
