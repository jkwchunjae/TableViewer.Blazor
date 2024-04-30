using System.Globalization;
using System.Numerics;
namespace TableViewerBlazor.Options.Property;

// https://mudblazor.com/api/numericfield#properties
public interface ITeNumericFieldProperty
{
    object Min { get; }
    object Max { get; }
    bool Required { get; }
    string? Pattern { get; }
    
    // Behavior
    Adornment Adornment { get; }
    string? AdornmentIcon { get; }
    string? AdornmentText { get; }
    bool AutoFocus { get; }
    bool Clearable { get; }
    CultureInfo? Culture { get; }
    double DebounceInterval { get; }
    bool Disabled { get; }
    string? Format { get; }
    string? HelperText { get; }
    bool HelperTextOnFocus { get; }
    bool Immediate { get; }
    InputMode InputMode { get; }
    bool InvertMouseWheel { get; }
    string? Placeholder { get; }
    bool ReadOnly { get; }
    object Step { get; }

    // Appearance
    string? AdornmentAriaLabel { get; }
    Color AdornmentColor { get; }
    bool DisableUnderLine { get; }
    bool FullWidth { get; }
    bool HideSpinButtons { get; }
    Size IconSize { get; }
    Margin Margin { get; }
    bool ShrinkLabel { get; }
    Variant Variant { get; }

    // Common
    string? Class { get; }
    string? Style { get; }
}
public class TeNumericFieldProperty<T> : ITeNumericFieldProperty
    where T : INumber<T>, IMinMaxValue<T>
{
    public T Min { get; set; } = T.MinValue;
    public T Max { get; set; } = T.MaxValue;
    public bool Required { get; set; }
    public string? Pattern { get; set; } = @"[0-9,.\-]";
    public Adornment Adornment { get; set; } = Adornment.None;
    public string? AdornmentIcon { get; set; } = null;
    public string? AdornmentText { get; set; } = null;
    public bool AutoFocus { get; set; }
    public bool Clearable { get; set; }
    public CultureInfo? Culture { get; set; }
    public double DebounceInterval { get; set; } = 0;
    public bool Disabled { get; set; }
    public string? Format { get; set; }
    public string? HelperText { get; set; }
    public bool HelperTextOnFocus { get; set; }
    public bool Immediate { get; set; }
    public InputMode InputMode { get; set; } = InputMode.numeric;
    public bool InvertMouseWheel { get; set; }
    public string? Placeholder { get; set; }
    public bool ReadOnly { get; set; }
    public T Step { get; set; } = T.One;
    public string? AdornmentAriaLabel { get; set; }
    public Color AdornmentColor { get; set; } = Color.Default;
    public bool DisableUnderLine { get; set; }
    public bool FullWidth { get; set; }
    public bool HideSpinButtons { get; set; }
    public Size IconSize { get; set; } = Size.Medium;
    public Margin Margin { get; set; } = Margin.None;
    public bool ShrinkLabel { get; set; }
    public Variant Variant { get; set; } = Variant.Text;
    public string? Class { get; set; } = null;
    public string? Style { get; set; } = null;

    object ITeNumericFieldProperty.Min => Min;
    object ITeNumericFieldProperty.Max => Max;
    object ITeNumericFieldProperty.Step => Step;
}