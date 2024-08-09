namespace TableViewerBlazor.Options.Property;

public class TeDebouncedInputProperty : TeBaseInputProperty
{
    /// <summary>
    /// The number of milliseconds to wait before updating the <see cref="MudBaseInput{T}.Text"/> value.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public double DebounceInterval { get; set; }
}
