namespace TableViewerBlazor.Options.Property;

public class TeBaseInputProperty : TeFormComponentProperty
{
    /// <summary>
    /// Allows the component to receive input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// Prevents the input from being changed by the user.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the user can copy text in the control, but cannot change the <see cref="Value" />.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Fills the full width of the parent container.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool FullWidth { get; set; }

    /// <summary>
    /// Changes the <see cref="Value"/> as soon as input is received.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the <see cref="Value"/> property will be updated any time user input occurs.  Otherwise, <see cref="Value"/> is updated when the user presses <c>Enter</c> or the input loses focus.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Immediate { get; set; }

    /// <summary>
    /// Displays an underline for the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Underline { get; set; } = true;

    /// <summary>
    /// The ID of the helper element, for use by <c>aria-describedby</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  When set it is appended to the <c>aria-describedby</c> attribute to improve accessibility for users. This ID takes precedence over the helper element rendered when <see cref="HelperText"/> is provided.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Validation)]
    public string? HelperId { get; set; }

    /// <summary>
    /// The text displayed below the text field.
    /// </summary>
    /// <remarks>
    /// This property is typically used to help the user understand what kind of input is allowed.  The <see cref="HelperTextOnFocus"/> property controls when this text is visible.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? HelperText { get; set; }

    /// <summary>
    /// Displays the <see cref="HelperText"/> only when this input has focus.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool HelperTextOnFocus { get; set; }

    /// <summary>
    /// The icon displayed for the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This icon will be displayed when <see cref="Adornment"/> is <c>Start</c> or <c>End</c>, and no value for <see cref="AdornmentText"/> is set.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? AdornmentIcon { get; set; }

    /// <summary>
    /// The text displayed for the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This text will be displayed when <see cref="Adornment"/> is <c>Start</c> or <c>End</c>.  The <see cref="AdornmentIcon"/> property will be ignored if this property is set.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? AdornmentText { get; set; }

    /// <summary>
    /// The location of the adornment icon or text.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Adornment.None"/>.  When set to <c>Start</c> or <c>End</c>, the <see cref="AdornmentText"/> will be displayed, or <see cref="AdornmentIcon"/> if no adornment text is specified.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public Adornment Adornment { get; set; } = Adornment.None;

    /// <summary>
    /// Limits validation to when the user changes the <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>. When <c>true</c>, validation only occurs if the user has changed the input value at least once.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool OnlyValidateIfDirty { get; set; }

    /// <summary>
    /// The color of <see cref="AdornmentText"/> or <see cref="AdornmentIcon"/>.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Color.Default"/>.  Theme colors are supported.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color AdornmentColor { get; set; } = Color.Default;

    /// <summary>
    /// The <c>aria-label</c> for the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string? AdornmentAriaLabel { get; set; }

    /// <summary>
    /// The size of the icon.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Size.Medium"/>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Size IconSize { get; set; } = Size.Medium;

    /// <summary>
    /// The appearance variation to use.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Variant.Text"/>.  Other options are <c>Outlined</c> and <c>Filled</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Variant Variant { get; set; } = Variant.Text;

    /// <summary>
    /// The amount of vertical spacing for this input.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Margin.None"/>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Margin Margin { get; set; } = Margin.None;

    /// <summary>
    /// Typography for the input text.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Typo Typo { get; set; } = Typo.input;

    /// <summary>
    /// The text displayed in the input if no <see cref="Value"/> is specified.
    /// </summary>
    /// <remarks>
    /// This property is typically used to give the user a hint as to what kind of input is expected.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The optional character count and stop count.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  When <c>0</c>, the current character count is displayed.  When <c>1</c> or greater, the character count and this count are displayed.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Validation)]
    public int? Counter { get; set; }

    /// <summary>
    /// The maximum number of characters allowed.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>524288</c>.  This value is typically set to a maximum length such as the size of a database column the value will be persisted to.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Validation)]
    public int MaxLength { get; set; } = 524288;

    /// <summary>
    /// The label for this input.
    /// </summary>
    /// <remarks>
    /// If no <see cref="Value"/> is specified, the label will be displayed in the input.  Otherwise, it will be scaled down to the top of the input.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Label { get; set; }

    /// <summary>
    /// Automatically receives focus.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the input will receive focus automatically.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool AutoFocus { get; set; }

    /// <summary>
    ///  A multiline input (textarea) will be shown, if set to more than one line.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public int Lines { get; set; } = 1;

    /// <summary>
    /// The type of input expected.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="InputMode.text"/>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public virtual InputMode InputMode { get; set; } = InputMode.text;

    /// <summary>
    /// The regular expression used to validate the <see cref="Value"/> property.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This property is used to validate the input against a regular expression.  Not supported if <see cref="Lines"/> is <c>2</c> or greater.  Must be a valid JavaScript regular expression.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Validation)]
    public virtual string? Pattern { get; set; }

    /// <summary>
    /// Shows the label inside the input if no <see cref="Value"/> is specified.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the label will not move into the input when the input is empty.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool ShrinkLabel { get; set; } = MudGlobal.InputDefaults.ShrinkLabel;

    /// <summary>
    /// The format applied to values.
    /// </summary>
    /// <remarks>
    /// This property is passed into the <c>ToString()</c> method of the <see cref="Value"/> property, such as formatting <c>int</c>, <c>float</c>, <c>DateTime</c> and <c>TimeSpan</c> values.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Format { get; set; }
}
