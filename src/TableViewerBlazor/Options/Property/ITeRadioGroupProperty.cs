using System.Globalization;

namespace TableViewerBlazor.Options.Property;

// https://mudblazor.com/api/radiogroup#properties
public interface ITeRadioGroupProperty
{
    bool Required { get; }
    CultureInfo? Culture { get; }
    bool Disabled { get; }
    bool ReadOnly { get; }
    string? InputClass { get; }
    string? InputStyle { get; }
    bool DisableUnderLine { get; }
}

public class TeRadioGroupProperty : ITeRadioGroupProperty
{
    public bool Required { get; set; } = false;
    public CultureInfo? Culture { get; set; }
    public bool Disabled { get; set; } = false;
    public bool ReadOnly { get; set; } = false;
    public string? InputClass { get; set; }
    public string? InputStyle { get; set; }
    public bool DisableUnderLine { get; set; } = false;
}
