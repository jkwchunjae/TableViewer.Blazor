namespace TableViewer.MewUI.Options.Property;

public class TeRadioGroupProperty : TeFormComponentProperty
{
    /// <summary>
    /// User class names for the input, separated by space
    /// </summary>
    public string? InputClass { get; set; }

    /// <summary>
    /// User style definitions for the input
    /// </summary>
    [Obsolete("MudBlazor no longer supports inline input styles; use InputClass with CSS instead.")]
    public string? InputStyle { get; set; }

    /// <summary>
    /// If true, the input will be disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, the input will be read-only.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Displays an underline for the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    public bool Underline { get; set; } = true;
}

