namespace TableViewer.MewUI.Options.Property;

public class TeDebouncedInputProperty : TeBaseInputProperty
{
    /// <summary>
    /// The number of milliseconds to wait before updating the <see cref="MudBaseInput{T}.Text"/> value.
    /// </summary>
    public double DebounceInterval { get; set; }
}

