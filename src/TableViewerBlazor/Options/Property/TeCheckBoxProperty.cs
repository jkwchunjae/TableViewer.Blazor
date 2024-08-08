namespace TableViewerBlazor.Options.Property;

public class TeCheckBoxProperty : TeBooleanInputProperty
{
    /// <summary>
    /// The color of the checkbox.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Color.Default"/>.  Theme colors are supported.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// The color of the checkbox when its <c>Value</c> is <c>false</c> or <c>null</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  Theme colors are supported.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.Radio.Appearance)]
    public Color? UncheckedColor { get; set; } = null;

    /// <summary>
    /// The text to display next to the checkbox.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Label { get; set; }

    /// <summary>
    /// The position of the <see cref="Label" /> text.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="LabelPosition.End"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.End;

    /// <summary>
    /// Allows this checkbox to be controlled via the keyboard.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.  The <c>Space</c> key cycles through true and false values (or true/false/null states if <see cref="TriState"/> is <c>true</c>). <c>Delete</c> will clear the checkbox. <c>Enter</c> (or <c>NumPadEnter</c>) will set the checkbox.  <c>Backspace</c> will set an indeterminate value.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool KeyboardEnabled { get; set; } = true;

    /// <summary>
    /// Shows a ripple effect when this checkbox is clicked.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Ripple { get; set; } = true;

    /// <summary>
    /// Uses compact padding.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Dense { get; set; }

    /// <summary>
    /// The size of the checkbox.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Size.Medium"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// The icon to display for a checked state.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Icons.Material.Filled.CheckBox"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string CheckedIcon { get; set; } = Icons.Material.Filled.CheckBox;

    /// <summary>
    /// The icon to display for an unchecked state.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Icons.Material.Filled.CheckBoxOutlineBlank"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string UncheckedIcon { get; set; } = Icons.Material.Filled.CheckBoxOutlineBlank;

    /// <summary>
    /// The icon to display for an indeterminate state.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Icons.Material.Filled.IndeterminateCheckBox"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string IndeterminateIcon { get; set; } = Icons.Material.Filled.IndeterminateCheckBox;

    /// <summary>
    /// Allows the checkbox to have an indeterminate state.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the checkbox can support an indeterminate state such as a <c>null</c> value, in addition to <c>true</c> and <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public bool TriState { get; set; }

    /// <summary>
    /// Displays an underline for the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Underline { get; set; } = true;
}