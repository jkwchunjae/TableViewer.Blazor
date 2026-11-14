namespace TableViewer.MewUI.Options.Property;

public class TeSelectBoxProperty : TeBaseInputProperty
{
    /// <summary>
    /// The outer div's classnames, seperated by space.
    /// </summary>
    public string? OuterClass { get; set; }

    /// <summary>
    /// Input's classnames, seperated by space.
    /// </summary>
    public string? InputClass { get; set; }

    /// <summary>
    /// User class names for the popover, separated by space
    /// </summary>
    public string? PopoverClass { get; set; }

    /// <summary>
    /// User class names for the internal list, separated by space
    /// </summary>
    public string? ListClass { get; set; }

    /// <summary>
    /// If true, compact vertical padding will be applied to all Select items.
    /// </summary>
    public bool Dense { get; set; }

    /// <summary>
    /// The Open Select Icon
    /// </summary>
    public string OpenIcon { get; set; } = "●";

    /// <summary>
    /// The Close Select Icon
    /// </summary>
    public string CloseIcon { get; set; } = "●";

    /// <summary>
    /// If set to true and the MultiSelection option is set to true, a "select all" checkbox is added at the top of the list of items.
    /// </summary>
    public bool SelectAll { get; set; }

    /// <summary>
    /// Define the text of the Select All option.
    /// </summary>
    public string SelectAllText { get; set; } = "Select all";
    /// <summary>
    /// Function to define a customized multiselection text.
    /// </summary>
    public Func<List<string>, string>? MultiSelectionTextFunc { get; set; }

    /// <summary>
    /// Parameter to define the delimited string separator.
    /// </summary>
    public string Delimiter { get; set; } = ", ";

    /// <summary>
    /// Sets the maxheight the Select can have when open.
    /// </summary>
    public int MaxHeight { get; set; } = 300;

    /// <summary>
    /// Set the anchor origin point to determen where the popover will open from.
    /// </summary>
    public Origin AnchorOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// Sets the transform origin point for the popover.
    /// </summary>
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// If true, the Select's input will not show any values that are not defined in the dropdown.
    /// This can be useful if Value is bound to a variable which is initialized to a value which is not in the list
    /// and you want the Select to show the label / placeholder instead.
    /// </summary>
    public bool Strict { get; set; }

    /// <summary>
    /// Show clear button.
    /// </summary>
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// Custom clear icon when <see cref="Clearable"/> is enabled.
    /// </summary>
    public string ClearIcon { get; set; } = "●";

    /// <summary>
    /// If true, prevent scrolling while dropdown is open.
    /// </summary>
    public bool LockScroll { get; set; } = false;

    /// <summary>
    /// Custom checked icon.
    /// </summary>
    public string CheckedIcon { get; set; } = "●";

    /// <summary>
    /// Custom unchecked icon.
    /// </summary>
    public string UncheckedIcon { get; set; } = "●";

    /// <summary>
    /// Custom indeterminate icon.
    /// </summary>
    public string IndeterminateIcon { get; set; } = "●";
}

public class TeSelectBoxProperty<T> : TeSelectBoxProperty
{
}
