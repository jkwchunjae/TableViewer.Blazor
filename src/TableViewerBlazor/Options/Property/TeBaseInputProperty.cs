using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Options.Property;

public class TeBaseInputProperty : ITeFormComponentProperty
{
    // https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Base/MudBaseInput.cs

    /// <summary>
    /// Gets or sets whether this input fills the full width of its container.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool FullWidth { get; set; }

    /// <summary>
    /// Gets or sets whether the <see cref="Value"/> is changed as soon as input is received.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the <see cref="Value"/> property will be updated any time user input occurs.  Otherwise, <see cref="Value"/> is updated when the user presses <c>Enter</c> or the input loses focus.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Immediate { get; set; }

    /// <summary>
    /// Gets or sets whether the input has an underline.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Underline { get; set; } = true;

    /// <summary>
    /// Gets or sets the text displayed below the text field.
    /// </summary>
    /// <remarks>
    /// This property is typically used to help the user understand what kind of input is allowed.  The <see cref="HelperTextOnFocus"/> property controls when this text is visible.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? HelperText { get; set; }

    /// <summary>
    /// Gets or sets whether the <see cref="HelperText"/> is only shown when this input has focus.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool HelperTextOnFocus { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed for the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This icon will be displayed when <see cref="Adornment"/> is <c>Start</c> or <c>End</c>, and no value for <see cref="AdornmentText"/> is set.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? AdornmentIcon { get; set; }

    /// <summary>
    /// Gets or sets the text displayed for the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This text will be displayed when <see cref="Adornment"/> is <c>Start</c> or <c>End</c>.  The <see cref="AdornmentIcon"/> property will be ignored if this property is set.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? AdornmentText { get; set; }

    /// <summary>
    /// Gets or sets the location of the adornment icon or text.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Adornment.None"/>.  Then set to <c>Start</c> or <c>End</c>, the <see cref="AdornmentText"/> will be displayed, or <see cref="AdornmentIcon"/> if no adornment text is specified.  
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public Adornment Adornment { get; set; } = Adornment.None;

    /// <summary>
    /// Gets whether validation only occurs when the user changes the <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>. When <c>true</c>, validation only occurs if the user has changed the input value at least once.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool OnlyValidateIfDirty { get; set; }

    /// <summary>
    /// Gets or sets the color of <see cref="AdornmentText"/> or <see cref="AdornmentIcon"/>.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Color.Default"/>.  Theme colors are supported.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Color AdornmentColor { get; set; } = Color.Default;

    /// <summary>
    /// Gets or sets the ARIA label of the adornment.
    /// </summary>
    /// <remarks>
    /// Defaults to an empty string.  This property controls the value set for the <c>aria-label</c> attribute.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string AdornmentAriaLabel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    /// <remarks>
    /// Default to <see cref="Size.Medium"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Size IconSize { get; set; } = Size.Medium;

    /// <summary>
    /// Gets or sets the appearance variation to use.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Variant.Text"/>.  Other options are <c>Outlined</c> and <c>Filled</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Variant Variant { get; set; } = Variant.Text;

    /// <summary>
    /// Gets or sets the amount of vertical spacing for this input.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Margin.None"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Margin Margin { get; set; } = Margin.None;

    /// <summary>
    /// Gets or sets the text displayed in the input if no <see cref="Value"/> is specified.
    /// </summary>
    /// <remarks>
    /// This property is typically used to give the user a hint as to what kind of input is expected.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets an optional character count and stop count.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  When <c>0</c>, the current character count is displayed.  When <c>1</c> or greater, the character count and this count are displayed.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public int? Counter { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of characters allowed.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>524288</c>.  This value is typically set to a maximum length such as the size of a database column the value will be persisted to.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public int MaxLength { get; set; } = 524288;

    /// <summary>
    /// Gets or sets the label for this input.
    /// </summary>
    /// <remarks>
    /// If no <see cref="Value"/> is specified, the label will be displayed in the input.  Otherwise, it will be scaled down to the top of the input.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets whether this input automatically receives focus.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the input will receive focus automatically.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool AutoFocus { get; set; }

    /// <summary>
    ///  A multiline input (textarea) will be shown, if set to more than one line.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public int Lines { get; set; } = 1;

    /// <summary>
    /// Gets or sets the text displayed in the input.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Data)]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets whether the text cannot be updated via a bound value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.  Applies only to Blazor Server (BSS) applications.  When <c>false</c>, the input's text can be updated programmatically while the input has focus.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool TextUpdateSuppression { get; set; } = true; // Solves issue #1012: Textfield swallowing chars when typing rapidly

    /// <summary>
    /// Gets or sets the type of input expected.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="InputMode.text"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public virtual InputMode InputMode { get; set; } = InputMode.text;

    /// <summary>
    /// Gets or sets the regular expression used to validate the <see cref="Value"/> property.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  This property is used to validate the input against a regular expression.  Not supported if <see cref="Lines"/> is <c>2</c> or greater.  Must be a valid JavaScript regular expression.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public virtual string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets whether the label is allowed to appear inside the input if no <see cref="Value"/> is specified.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the label will not move into the input when the input is empty.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool ShrinkLabel { get; set; }



    /// <summary>
    /// Gets or sets the format applied to values.
    /// </summary>
    /// <remarks>
    /// This property is passed into the <c>ToString()</c> method of the <see cref="Value"/> property, such as formatting <c>int</c>, <c>float</c>, <c>DateTime</c> and <c>TimeSpan</c> values.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? Format { get; set; }

    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string? InputId { get; set; }
}
