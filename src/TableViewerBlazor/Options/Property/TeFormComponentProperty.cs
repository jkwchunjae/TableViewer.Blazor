using System.Globalization;

namespace TableViewerBlazor.Options.Property;

public class TeFormComponentProperty
{
    // https://github.com/MudBlazor/MudBlazor/blob/dev/src/MudBlazor/Base/MudFormComponent.cs

    /// <summary>
    /// Gets or sets whether an input is required.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, an error with the text in <see cref="RequiredError"/> will be shown during validation if no input was given.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public bool Required { get; set; }


    /// <summary>
    /// Gets or sets the culture used to format and interpret values such as dates and currency.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="CultureInfo.InvariantCulture"/>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
}
