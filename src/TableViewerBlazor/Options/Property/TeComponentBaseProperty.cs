namespace TableViewerBlazor.Options.Property;

public class TeComponentBaseProperty
{
    /// <summary>
    /// The CSS classes applied to this component.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  You can use spaces to separate multiple classes.  Use the <see cref="Style"/> property to apply custom CSS styles.
    /// </remarks>
    [Category(CategoryTypes.ComponentBase.Common)]
    public string? Class { get; set; }

    /// <summary>
    /// The CSS styles applied to this component.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  Use the <see cref="Class"/> property to apply CSS classes.
    /// </remarks>
    [Category(CategoryTypes.ComponentBase.Common)]
    public string? Style { get; set; }

    /// <summary>
    /// The arbitrary object to link to this component.
    /// </summary>
    /// <remarks>
    /// This property is typically used to associate additional information with this component, such as a model containing data for this component.
    /// </remarks>
    [Category(CategoryTypes.ComponentBase.Common)]
    public object? Tag { get; set; }

    /// <summary>
    /// The additional HTML attributes to apply to this component.
    /// </summary>
    /// <remarks>
    /// This property is typically used to provide additional HTML attributes during rendering such as ARIA accessibility tags or a custom ID.
    /// </remarks>
    [Category(CategoryTypes.ComponentBase.Common)]
    public Dictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();
}
