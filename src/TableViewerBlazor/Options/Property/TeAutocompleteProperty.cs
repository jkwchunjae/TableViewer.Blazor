namespace TableViewerBlazor.Options.Property;

public class TeAutocompleteProperty : TeBaseInputProperty
{
    /// <summary>
    /// The CSS classes applied to the popover. Defaults to null. You can use spaces to separate multiple classes.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string? PopoverClass { get; set; }

    /// <summary>
    /// The CSS classes applied to the internal list. Defaults to null. You can use spaces to separate multiple classes.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string? ListClass { get; set; }

    /// <summary>
    /// Uses compact padding. Defaults to false.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public bool Dense { get; set; } = false;

    /// <summary>
    /// The CSS classes applied to internal list items. Defaults to null. You can use spaces to separate multiple classes.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string? ListItemClass { get; set; } = null;


    /// <summary>
    /// The custom template shown below the list of items, if SearchFunc returns items to display.Otherwise, the fragment is hidden. Defaults to null. Use the BeforeItemsTemplate property to control content displayed above items.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment AfterItemsTemplate { get; set; } = null!;

    /// <summary>
    /// The custom template shown above the list of items, if SearchFunc returns items to display.Otherwise, the fragment is hidden.Defaults to null. Use the AfterItemsTemplate property to control content displayed below items.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment BeforeItemsTemplate { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public int? MaxItems { get; set; } = 10;

    /// <summary>
    /// The custom template used when the number of items returned by SearchFunc is more than the value of the MaxItems property.Defaults to null.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment MoreItemsTemplate { get; set; } = null!;

    /// <summary>
    /// The custom template used when no items are returned by SearchFunc. Defaults to null.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment NoItemsTemplate { get; set; } = null!;
    
    /// <summary>
    /// The custom template used for the progress indicator inside the popover when ShowProgressIndicator is true. Defaults to null. Use the ProgressIndicatorTemplate property to control content displayed for the progress indicator.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment ProgressIndicatorInPopoverTemplate { get; set; } = null!;

    /// <summary>
    /// The custom template used for the progress indicator when ShowProgressIndicator is true. Defaults to null. Use the ProgressIndicatorInPopoverTemplate property to control content displayed for the progress indicator inside the popover.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public RenderFragment ProgressIndicatorTemplate { get; set; } = null!;

    /// <summary>
    /// The "open" Autocomplete icon.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string OpenIcon { get; set; } = MudBlazor.Icons.Material.Filled.ArrowDropDown;

    /// <summary>
    /// The "close" Autocomplete icon. 
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string CloseIcon { get; set; } = MudBlazor.Icons.Material.Filled.ArrowDropDown;

    /// <summary>
    /// Custom clear icon when Clearable is enabled.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string? ClearIcon { get; set; }


    /// <summary>
    /// The maximum height, in pixels, of the Autocomplete when it is open. Defaults to 300.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public int MaxHeight { get; set; } = 300;

    /// <summary>
    /// The location where the popover will open from.Defaults to BottomCenter.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin AnchorOrigin { get; set; } = Origin.BottomCenter;

    /// <summary>
    /// The transform origin point for the popover. Defaults to TopCenter.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;


    /// <summary>
    /// Selects items without resetting the Value. Defaults to true. When true, selecting an option will trigger a SearchFunc with the current Text.Otherwise, an empty string is passed which can make it easier to view and select other options without resetting the Value. When false, T must either be a record or override the GetHashCode and Equals methods.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Strict { get; set; } = true;


    /// <summary>
    /// Displays the Clear icon button. Defaults to false. When true, an icon is displayed which, when clicked, clears the Text and Value. Use the ClearIcon property to control the Clear button icon.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// Reset the selected value if the user deletes the text. Defaults to false.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ResetValueOnEmptyText { get; set; } = false;

    /// <summary>
    /// Overrides the Text property when an item is selected. Defaults to true. When true, selecting a value will update the Text property. When false, incomplete values for Text are allowed.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool CoerceText { get; set; } = true;

    /// <summary>
    /// Sets the Value property even if no match is found by SearchFunc. Defaults to false. When true, the user input will be applied to the Value property which allows it to be validated and show an error message.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool CoerceValue { get; set; } = false;


    /// <summary>
    /// The debounce interval, in milliseconds. Defaults to 100. A higher value can help reduce the number of calls to SearchFunc, which can improve responsiveness.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public int DebounceInterval { get; set; } = 100;

    /// <summary>
    /// The minimum number of characters typed to initiate a search. Defaults to 0.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public int MinCharacters { get; set; } = 0;


    /// <summary>
    /// Shows the progress indicator during searches. Defaults to false. The progress indicator uses the color specified in the ProgressIndicatorColor property.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ShowProgressIndicator { get; set; } = false;

    /// <summary>
    /// Prevents the text from being updated via a bound value. Defaults to true. Applies only to Blazor Server (BSS) applications. When false, the input's text can be updated programmatically while the input has focus.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool TextUpdateSuppression { get; set; } = true;
}
