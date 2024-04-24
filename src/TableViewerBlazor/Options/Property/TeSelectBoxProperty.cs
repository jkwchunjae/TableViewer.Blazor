using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Options.Property;

public class TeSelectBoxProperty : TeBaseInputProperty
{

    /// <summary>
    /// The outer div's classnames, seperated by space.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    [Parameter] public string OuterClass { get; set; }

    /// <summary>
    /// Input's classnames, seperated by space.
    /// </summary>
    [Category(CategoryTypes.FormComponent.Appearance)]
    [Parameter] public string InputClass { get; set; }


    /// <summary>
    /// User class names for the popover, separated by space
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string PopoverClass { get; set; }

    /// <summary>
    /// User class names for the internal list, separated by space
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string ListClass { get; set; }

    /// <summary>
    /// If true, compact vertical padding will be applied to all Select items.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public bool Dense { get; set; }

    /// <summary>
    /// The Open Select Icon
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string OpenIcon { get; set; } = Icons.Material.Filled.ArrowDropDown;

    /// <summary>
    /// The Close Select Icon
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string CloseIcon { get; set; } = Icons.Material.Filled.ArrowDropUp;

    /// <summary>
    /// If set to true and the MultiSelection option is set to true, a "select all" checkbox is added at the top of the list of items.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public bool SelectAll { get; set; }

    /// <summary>
    /// Define the text of the Select All option.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string SelectAllText { get; set; } = "Select all";

    /// <summary>
    /// Function to define a customized multiselection text.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public Func<List<string>, string>? MultiSelectionTextFunc { get; set; }

    /// <summary>
    /// Parameter to define the delimited string separator.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public string Delimiter { get; set; } = ", ";


    /// <summary>
    /// Set of selected values. If MultiSelection is false it will only ever contain a single value. This property is two-way bindable.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Data)]
    public IEnumerable<T> SelectedValues { get; set; }

    /// <summary>
    /// The Comparer to use for comparing selected values internally.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public IEqualityComparer<T> Comparer { get; set; }


    /// <summary>
    /// Defines how values are displayed in the drop-down list
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public Func<T, string> ToStringFunc { get; set; }

    /// <summary>
    /// If true, multiple values can be selected via checkboxes which are automatically shown in the dropdown
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public bool MultiSelection { get; set; }


    /// <summary>
    /// Sets the maxheight the Select can have when open.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public int MaxHeight { get; set; } = 300;

    /// <summary>
    /// Set the anchor origin point to determen where the popover will open from.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin AnchorOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// Sets the transform origin point for the popover.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public Origin TransformOrigin { get; set; } = Origin.TopCenter;

    /// <summary>
    /// If true, the Select's input will not show any values that are not defined in the dropdown.
    /// This can be useful if Value is bound to a variable which is initialized to a value which is not in the list
    /// and you want the Select to show the label / placeholder instead.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Strict { get; set; }

    /// <summary>
    /// Show clear button.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// If true, prevent scrolling while dropdown is open.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListBehavior)]
    public bool LockScroll { get; set; } = false;


    /// <summary>
    /// Custom checked icon.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string CheckedIcon { get; set; } = Icons.Material.Filled.CheckBox;

    /// <summary>
    /// Custom unchecked icon.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string UncheckedIcon { get; set; } = Icons.Material.Filled.CheckBoxOutlineBlank;

    /// <summary>
    /// Custom indeterminate icon.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.ListAppearance)]
    public string IndeterminateIcon { get; set; } = Icons.Material.Filled.IndeterminateCheckBox;



    /// <summary>
    /// Gets or sets whether the component can receive input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Disabled { get; set; }


    /// <summary>
    /// Gets or sets whether the input can be changed by the user.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the user can copy text in the control, but cannot change the <see cref="Value" />.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool ReadOnly { get; set; }





}
