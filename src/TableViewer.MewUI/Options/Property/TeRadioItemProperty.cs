namespace TableViewer.MewUI.Options.Property;

public class TeRadioItemProperty : TeComponentBaseProperty
{
    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// The base color of the component in its none active/unchecked state. It supports the theme colors.
    /// </summary>
    public Color? UncheckedColor { get; set; } = null;

    /// <summary>
    /// The position of the child content.
    /// </summary>
    public Placement Placement { get; set; } = Placement.End;

    /// <summary>
    /// If true, compact padding will be applied.
    /// </summary>
    public bool Dense { get; set; }

    /// <summary>
    /// The Size of the component.
    /// </summary>
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// Gets or sets whether to show a ripple effect when the user clicks the button. Default is true.
    /// </summary>
    public bool Ripple { get; set; } = true;

    /// <summary>
    /// If true, the button will be disabled.
    /// </summary>
    public bool Disabled { get; set; }
}

