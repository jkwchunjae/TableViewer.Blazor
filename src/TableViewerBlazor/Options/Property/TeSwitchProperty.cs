namespace TableViewerBlazor.Options.Property;

public class TeSwitchProperty : TeBooleanInputProperty
{
    /// <summary>
    /// The text/label will be displayed next to the switch if set.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>;
    /// </remarks>
    public string? Label { get; set; }

    /// <summary>
    /// The position of the text/label.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="LabelPosition.End"/>
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.End;

    /// <summary>
    /// Show the text/label.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>
    /// </remarks>
    public bool ShowLabel { get; set; } = true;


    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>
    /// /// <remarks>
    /// Defaults to <see cref="Color.Primary"/>
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color Color { get; set; } = Color.Primary;

    /// <summary>
    /// Gets or sets whether to show a ripple effect when the user clicks the button. Default is true.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Ripple { get; set; }

    /// <summary>
    /// The Size of the switch.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Size.Medium"/>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Size Size { get; set; }

    /// <summary>
    /// Shows an icon on Switch's thumb when checked.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string ThumbIcon { get; set; } = string.Empty;

    /// <summary>
    /// Shows an icon on Switch's thumb when unchecked.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string ThumbIconUnChecked { get; set; } = string.Empty;

    /// <summary>
    /// The color of the thumb icon. Supports the theme colors.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color ThumbIconColor { get; set; }

    /// <summary>
    /// The base color of the component in its none active/unchecked state. It supports the theme colors.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color UncheckedColor { get; set; }

    /// <summary>
    /// Displays an underline for the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Underline { get; set; } = true;
}
