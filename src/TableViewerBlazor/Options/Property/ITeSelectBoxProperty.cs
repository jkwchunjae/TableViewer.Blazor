using Microsoft.AspNetCore.Components.Web;

namespace TableViewerBlazor.Options.Property;

public interface ITeSelectBoxProperty : ITeBaseInputProperty
{

    /// <summary>
    /// The outer div's classnames, seperated by space.
    /// </summary>
    string? OuterClass { get; }
    /// <summary>
    /// Input's classnames, seperated by space.
    /// </summary>
    string? InputClass { get; }

    /// <summary>
    /// User class names for the popover, separated by space
    /// </summary>
    string? PopoverClass { get; }
    /// <summary>
    /// User class names for the internal list, separated by space
    /// </summary>
    string? ListClass { get; }
    /// <summary>
    /// If true, compact vertical padding will be applied to all Select items.
    /// </summary>
    bool Dense { get; }
    /// <summary>
    /// The Open Select Icon
    /// </summary>
    string? OpenIcon { get; }

    /// <summary>
    /// The Close Select Icon
    /// </summary>
    string? CloseIcon { get; }

    /// <summary>
    /// If set to true and the MultiSelection option is set to true, a "select all" checkbox is added at the top of the list of items.
    /// </summary>
    bool SelectAll { get; }
    /// <summary>
    /// Define the text of the Select All option.
    /// </summary>
    string? SelectAllText { get; }

    /// <summary>
    /// Function to define a customized multiselection text.
    /// </summary>
    Func<List<string>, string>? MultiSelectionTextFunc { get; }
    /// <summary>
    /// Parameter to define the delimited string? separator.
    /// </summary>
    string? Delimiter { get; }


    /// <summary>
    /// Set of selected values. If MultiSelection is false it will only ever contain a single value. This property is two-way bindable.
    /// </summary>
    IEnumerable<object>? SelectedValues { get; }
    /// <summary>
    /// The Comparer to use for comparing selected values internally.
    /// </summary>
    IEqualityComparer<object>? Comparer { get; }

    /// <summary>
    /// Defines how values are displayed in the drop-down list
    /// </summary>
    Func<object, string>? ToStringFunc { get; }
    /// <summary>
    /// If true, multiple values can be selected via checkboxes which are automatically shown in the dropdown
    /// </summary>
    bool MultiSelection { get; }

    /// <summary>
    /// Sets the maxheight the Select can have when open.
    /// </summary>
    int MaxHeight { get; }

    /// <summary>
    /// Set the anchor origin point to determen where the popover will open from.
    /// </summary>
    Origin AnchorOrigin { get; }

    /// <summary>
    /// Sets the transform origin point for the popover.
    /// </summary>
    Origin TransformOrigin { get; }

    /// <summary>
    /// If true, the Select's input will not show any values that are not defined in the dropdown.
    /// This can be useful if Value is bound to a variable which is initialized to a value which is not in the list
    /// and you want the Select to show the label / placeholder instead.
    /// </summary>
    bool Strict { get; }
    /// <summary>
    /// Show clear button.
    /// </summary>
    bool Clearable { get; }

    /// <summary>
    /// If true, prevent scrolling while dropdown is open.
    /// </summary>
    bool LockScroll { get; }


    /// <summary>
    /// Custom checked icon.
    /// </summary>
    string? CheckedIcon { get; }

    /// <summary>
    /// Custom unchecked icon.
    /// </summary>
    string? UncheckedIcon { get; }

    /// <summary>
    /// Custom indeterminate icon.
    /// </summary>
    string? IndeterminateIcon { get; }



    /// <summary>
    /// Gets or sets whether the component can receive input.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.
    /// </remarks>
    bool Disabled { get; }

    /// <summary>
    /// Gets or sets whether the input can be changed by the user.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>.  When <c>true</c>, the user can copy text in the control, but cannot change the <see cref="Value" />.
    /// </remarks>
    bool ReadOnly { get; }




}
