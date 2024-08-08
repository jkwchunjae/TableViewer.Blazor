namespace TableViewerBlazor.Options.Property;

public class TeBooleanInputProperty : TeFormComponentProperty
{
    /// <summary>
    /// Prevents the user from interacting with this input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// Prevents the user from changing the input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the user can copy the input but cannot change it.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ReadOnly { get; set; }
}
