namespace TableViewerBlazor.Options.Property;

public class TeSelectBoxProperty : TeBaseInputProperty
{
    /// <summary>
    /// The outer div's classnames, seperated by space.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string? OuterClass { get; set; }

    /// <summary>
    /// Input's classnames, seperated by space.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string? InputClass { get; set; }

    /// <summary>
    /// User class names for the popover, separated by space
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string? PopoverClass { get; set; }

    /// <summary>
    /// User class names for the internal list, separated by space
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string? ListClass { get; set; }

    /// <summary>
    /// If true, compact vertical padding will be applied to all Select items.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public bool Dense { get; set; }

    /// <summary>
    /// The Open Select Icon
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string OpenIcon { get; set; } = Icons.Material.Filled.ArrowDropDown;

    /// <summary>
    /// The Close Select Icon
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string CloseIcon { get; set; } = Icons.Material.Filled.ArrowDropUp;

    /// <summary>
    /// If set to true and the MultiSelection option is set to true, a "select all" checkbox is added at the top of the list of items.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public bool SelectAll { get; set; }

    /// <summary>
    /// Define the text of the Select All option.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string SelectAllText { get; set; } = "Select all";
    /// <summary>
    /// Function to define a customized multiselection text.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public Func<List<string>, string>? MultiSelectionTextFunc { get; set; }

    /// <summary>
    /// Parameter to define the delimited string separator.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string Delimiter { get; set; } = ", ";

    /// <summary>
    /// Sets the maxheight the Select can have when open.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public int MaxHeight { get; set; } = 300;

    /// <summary>
    /// Set the anchor origin point to determen where the popover will open from.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin AnchorOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// Sets the transform origin point for the popover.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// If true, the Select's input will not show any values that are not defined in the dropdown.
    /// This can be useful if Value is bound to a variable which is initialized to a value which is not in the list
    /// and you want the Select to show the label / placeholder instead.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Strict { get; set; }

    /// <summary>
    /// Show clear button.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// Custom clear icon when <see cref="Clearable"/> is enabled.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string ClearIcon { get; set; } = Icons.Material.Filled.Clear;

    /// <summary>
    /// If true, prevent scrolling while dropdown is open.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public bool LockScroll { get; set; } = false;

    /// <summary>
    /// Custom checked icon.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string CheckedIcon { get; set; } = Icons.Material.Filled.CheckBox;

    /// <summary>
    /// Custom unchecked icon.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string UncheckedIcon { get; set; } = Icons.Material.Filled.CheckBoxOutlineBlank;

    /// <summary>
    /// Custom indeterminate icon.
    /// </summary>
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string IndeterminateIcon { get; set; } = Icons.Material.Filled.IndeterminateCheckBox;
}

public class TeSelectBoxProperty<T> : TeSelectBoxProperty
{
}