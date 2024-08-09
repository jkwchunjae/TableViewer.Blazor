namespace TableViewerBlazor.Options.Property;

public class TeRadioGroupProperty : TeFormComponentProperty
{
    /// <summary>
    /// User class names for the input, separated by space
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public string? InputClass { get; set; }

    /// <summary>
    /// User style definitions for the input
    /// </summary>
    [Category(CategoryTypes.Radio.Appearance)]
    public string? InputStyle { get; set; }

    /// <summary>
    /// If true, the input will be disabled.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, the input will be read-only.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Displays an underline for the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool Underline { get; set; } = true;
}
