namespace TableViewerBlazor.Options;


public interface ITeTextFieldProperty
{
    /// <summary>
    /// Type of the input element. It should be a valid HTML5 input type.
    /// </summary>
    InputType InputType { get; }

    /// <summary>
    /// Show clear button.
    /// </summary>
    bool Clearable { get; }

    /// <summary>
    /// Provide a masking object. Built-in masks are PatternMask, MultiMask, RegexMask and BlockMask
    /// Note: when Mask is set, TextField will ignore some properties such as Lines, Pattern or HideSpinButtons, OnKeyDown and OnKeyUp, etc.
    /// </summary>
    IMask? Mask { get; }

    /// <summary>
    /// Interval to be awaited in milliseconds before changing the Text value
    /// </summary>
    double DebounceInterval { get; }

    /// <summary>
    /// If true, the input element will be disabled.
    /// </summary>
    bool Disabled { get; }

    /// <summary>
    /// If true, the input will be read-only.
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>
    /// If true, the input will take up the full width of its container.
    /// </summary>
    bool FullWidth { get; }

    /// <summary>
    /// If true, the input will update the Value immediately on typing.
    /// If false, the Value is updated only on Enter.
    /// </summary>
    bool Immediate { get; }

    /// <summary>
    /// If true, the input will not have an underline.
    /// </summary>
    bool DisableUnderLine { get; }

    /// <summary>
    /// The HelperText will be displayed below the text field.
    /// </summary>
    string? HelperText { get; }

    /// <summary>
    /// If true, the helper text will only be visible on focus.
    /// </summary>
    bool HelperTextOnFocus { get; }

    /// <summary>
    /// Icon that will be used if Adornment is set to Start or End.
    /// </summary>
    string? AdornmentIcon { get; }

    /// <summary>
    /// Text that will be used if Adornment is set to Start or End, the Text overrides Icon.
    /// </summary>
    string? AdornmentText { get; }

    /// <summary>
    /// The Adornment if used. By default, it is set to None.
    /// </summary>
    Adornment Adornment { get; }

    /// <summary>
    /// The validation is only triggered if the user has changed the input value at least once. By default, it is false
    /// </summary>
    bool OnlyValidateIfDirty { get; }

    /// <summary>
    /// The color of the adornment if used. It supports the theme colors.
    /// </summary>
    Color AdornmentColor { get; }

    /// <summary>
    /// The aria-label of the adornment.
    /// </summary>
    string AdornmentAriaLabel { get; }

    /// <summary>
    /// The Icon Size.
    /// </summary>
    Size IconSize { get; }

    /// <summary>
    /// Variant to use.
    /// </summary>
    Variant Variant { get; }

    /// <summary>
    ///  Will adjust vertical spacing.
    /// </summary>
    Margin Margin { get; }

    /// <summary>
    /// The short hint displayed in the input before the user enters a value.
    /// </summary>
    string? Placeholder { get; }

    /// <summary>
    /// If set, will display the counter, value 0 will display current count but no stop count.
    /// </summary>
    int? Counter { get; }

    /// <summary>
    /// Maximum number of characters that the input will accept
    /// </summary>
    int MaxLength { get; }

    /// <summary>
    /// If string has value the label text will be displayed in the input, and scaled down at the top if the input has value.
    /// </summary>
    string? Label { get; }

    /// <summary>
    /// If true the input will focus automatically.
    /// </summary>
    bool AutoFocus { get; }

    /// <summary>
    ///  A multiline input (textarea) will be shown, if set to more than one line.
    /// </summary>
    int Lines { get; }

    /// <summary>
    /// When TextUpdateSuppression is true (which is default) the text can not be updated by bindings while the component is focused in BSS (not WASM).
    /// This solves issue #1012: Textfield swallowing chars when typing rapidly
    /// If you need to update the input's text while it is focused you can set this parameter to false.
    /// Note: on WASM text update suppression is not active, so this parameter has no effect.
    /// </summary>
    bool TextUpdateSuppression { get; }

    /// <summary>
    ///  Hints at the type of data that might be entered by the user while editing the input
    /// </summary>
    InputMode InputMode { get; }

    /// <summary>
    /// The pattern attribute, when specified, is a regular expression which the input's value must match in order for the value to pass constraint validation. It must be a valid JavaScript regular expression
    /// Not Supported in multline input
    /// </summary>
    string? Pattern { get; }

    /// <summary>
    /// ShrinkLabel prevents the label from moving down into the field when the field is empty.
    /// </summary>
    bool ShrinkLabel { get; }
}

public class TeTextFieldProperty : ITeTextFieldProperty
{
    /// <summary>
    /// Type of the input element. It should be a valid HTML5 input type.
    /// </summary>
    public InputType InputType { get; set; }

    /// <summary>
    /// Show clear button.
    /// </summary>
    public bool Clearable { get; set; }

    /// <summary>
    /// Provide a masking object. Built-in masks are PatternMask, MultiMask, RegexMask and BlockMask
    /// Note: when Mask is set, TextField will ignore some properties such as Lines, Pattern or HideSpinButtons, OnKeyDown and OnKeyUp, etc.
    /// </summary>
    public IMask? Mask { get; set; }

    /// <summary>
    /// Interval to be awaited in milliseconds before changing the Text value
    /// </summary>
    public double DebounceInterval { get; set; }

    /// <summary>
    /// If true, the input element will be disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, the input will be read-only.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// If true, the input will take up the full width of its container.
    /// </summary>
    public bool FullWidth { get; set; }

    /// <summary>
    /// If true, the input will update the Value immediately on typing.
    /// If false, the Value is updated only on Enter.
    /// </summary>
    public bool Immediate { get; set; }

    /// <summary>
    /// If true, the input will not have an underline.
    /// </summary>
    public bool DisableUnderLine { get; set; }

    /// <summary>
    /// The HelperText will be displayed below the text field.
    /// </summary>
    public string? HelperText { get; set; }

    /// <summary>
    /// If true, the helper text will only be visible on focus.
    /// </summary>
    public bool HelperTextOnFocus { get; set; }

    /// <summary>
    /// Icon that will be used if Adornment is set to Start or End.
    /// </summary>
    public string? AdornmentIcon { get; set; }

    /// <summary>
    /// Text that will be used if Adornment is set to Start or End, the Text overrides Icon.
    /// </summary>
    public string? AdornmentText { get; set; }

    /// <summary>
    /// The Adornment if used. By default, it is set to None.
    /// </summary>
    public Adornment Adornment { get; set; } = Adornment.None;

    /// <summary>
    /// The validation is only triggered if the user has changed the input value at least once. By default, it is false
    /// </summary>
    public bool OnlyValidateIfDirty { get; set; } = false;

    /// <summary>
    /// The color of the adornment if used. It supports the theme colors.
    /// </summary>
    public Color AdornmentColor { get; set; } = Color.Default;

    /// <summary>
    /// The aria-label of the adornment.
    /// </summary>
    public string AdornmentAriaLabel { get; set; } = string.Empty;

    /// <summary>
    /// The Icon Size.
    /// </summary>
    public Size IconSize { get; set; } = Size.Medium;

    /// <summary>
    /// Variant to use.
    /// </summary>
    public Variant Variant { get; set; } = Variant.Text;

    /// <summary>
    ///  Will adjust vertical spacing.
    /// </summary>
    public Margin Margin { get; set; } = Margin.None;

    /// <summary>
    /// The short hint displayed in the input before the user enters a value.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// If set, will display the counter, value 0 will display current count but no stop count.
    /// </summary>
    public int? Counter { get; set; }

    /// <summary>
    /// Maximum number of characters that the input will accept
    /// </summary>
    public int MaxLength { get; set; } = 524288;

    /// <summary>
    /// If string has value the label text will be displayed in the input, and scaled down at the top if the input has value.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// If true the input will focus automatically.
    /// </summary>
    public bool AutoFocus { get; set; }

    /// <summary>
    ///  A multiline input (textarea) will be shown, if set to more than one line.
    /// </summary>
    public int Lines { get; set; } = 1;

    /// <summary>
    /// When TextUpdateSuppression is true (which is default) the text can not be updated by bindings while the component is focused in BSS (not WASM).
    /// This solves issue #1012: Textfield swallowing chars when typing rapidly
    /// If you need to update the input's text while it is focused you can set this parameter to false.
    /// Note: on WASM text update suppression is not active, so this parameter has no effect.
    /// </summary>
    public bool TextUpdateSuppression { get; set; } = true;

    /// <summary>
    ///  Hints at the type of data that might be entered by the user while editing the input
    /// </summary>
    public InputMode InputMode { get; set; } = InputMode.text;

    /// <summary>
    /// The pattern attribute, when specified, is a regular expression which the input's value must match in order for the value to pass constraint validation. It must be a valid JavaScript regular expression
    /// Not Supported in multline input
    /// </summary>
    public string? Pattern { get; set; }

    /// <summary>
    /// ShrinkLabel prevents the label from moving down into the field when the field is empty.
    /// </summary>
    public bool ShrinkLabel { get; set; } = false;
}
