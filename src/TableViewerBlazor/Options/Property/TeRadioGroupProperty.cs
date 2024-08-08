namespace TableViewerBlazor.Options.Property;

public class TeRadioGroupProperty : TeFormComponentProperty
{
    /// <summary>
    /// User class names for the input, separated by space
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Radio.Appearance)]
    public string? InputClass { get; set; }

    /// <summary>
    /// User style definitions for the input
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Radio.Appearance)]
    public string? InputStyle { get; set; }

    /// <summary>
    /// If true, the input will be disabled.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, the input will be read-only.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ReadOnly { get; set; }

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
