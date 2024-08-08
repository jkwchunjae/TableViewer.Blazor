using System.Globalization;
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
    string? Label { get; }
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
    bool Underline { get; }
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
