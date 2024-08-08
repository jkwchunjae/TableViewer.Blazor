namespace TableViewerBlazor.Options.Property;

public class TeFormComponentProperty : TeComponentBaseProperty
{
    /// <summary>
    /// Requires an input value.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, an error with the text in <see cref="RequiredError"/> will be shown during validation if no input was given.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public bool Required { get; set; }

    /// <summary>
    /// The text displayed during validation if no input was given.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>"Required"</c>.  This text is only shown when <see cref="Required"/> is <c>true</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public string RequiredError { get; set; } = "Required";

    /// <summary>
    /// The text displayed if the <see cref="Error"/> property is <c>true</c>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public string? ErrorText { get; set; }

    /// <summary>
    /// Displays an error.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the text in <see cref="ErrorText"/> is displayed.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public bool Error { get; set; }

    /// <summary>
    /// The ID of the error description element, for use by <c>aria-describedby</c> when <see cref="Error"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>null</c>.  When set and the <see cref="Error"/> property is <c>true</c>, an <c>aria-describedby</c> attribute is rendered to improve accessibility for users.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public string? ErrorId { get; set; }
}
