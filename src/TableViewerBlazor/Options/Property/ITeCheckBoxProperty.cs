﻿using System.Globalization;

namespace TableViewerBlazor.Options.Property;

public interface ITeCheckBoxProperty
{
    /// <summary> 
    /// Allows the checkbox to have an indeterminate state. Defaults to false. When true, the checkbox can support an indeterminate state such as a null value, in addition to true and false.
    /// </summary>
    public bool TriState { get; }
    
    /// <summary>
    /// Prevents the user from interacting with this input. Defaults to false.
    /// </summary>
    public bool Disabled { get; }
    
    /// <summary>
    /// Allows this checkbox to be controlled via the keyboard. Defaults to true. The Space key cycles through true and false values (or true/false/null states if TriState
    /// is true). Delete will clear the checkbox.Enter(or NumPadEnter) will set the checkbox.Backspace will set an indeterminate value.
    /// </summary>
    public bool KeyboardEnabled { get; }
    
    /// <summary>
    /// The text to display next to the checkbox. Defaults to null.
    /// </summary>
    public string? Label { get; }
    
    /// <summary>
    /// The position of the Label text. Defaults to End.
    /// </summary>
    public LabelPosition LabelPosition { get; }
    
    /// <summary>
    /// Prevents the user from changing the input. Defaults to false. When true, the user can copy the input but cannot change it.
    /// </summary>
    public bool ReadOnly { get; }
    
    /// <summary>
    /// The culture used to format and interpret values such as dates and currency. 
    /// </summary>
    public CultureInfo Culture { get; }
    
    /// <summary>
    /// The icon to display for a checked state
    /// </summary>
    public string CheckedIcon { get; }
    
    /// <summary>
    /// The color of the checkbox. Defaults to Default. Theme colors are supported.
    /// </summary>
    public Color Color { get; }
    
    /// <summary>
    /// Uses compact padding. Defaults to false.
    /// </summary>
    public bool Dense { get; }
    
    /// <summary>
    /// The icon to display for an indeterminate state
    /// </summary>
    public string IndeterminateIcon { get; }
    
    /// <summary>
    /// Shows a ripple effect when this checkbox is clicked. Defaults to true.
    /// </summary>
    public bool Ripple { get; }
    
    /// <summary>
    /// The size of the checkbox. Defaults to Medium
    /// </summary>
    public Size Size { get; }
    
    /// <summary>
    /// The color of the checkbox when its Value is false or null. Defaults to null. Theme colors are supported.
    /// </summary>
    public Color? UncheckedColor { get; }
    
    /// <summary>
    /// The icon to display for an unchecked state. 
    /// </summary>
    public string UncheckedIcon { get; }

    public bool HideText { get; }
}

public class TeCheckBoxProperty : ITeCheckBoxProperty
{
    public bool TriState { get; init; }
    public bool Disabled { get; init; }
    public bool KeyboardEnabled { get; init; } = true;
    public string? Label { get; init; }
    public LabelPosition LabelPosition { get; init;} = LabelPosition.End;
    public bool ReadOnly { get; init; }
    public CultureInfo Culture { get; init; } = System.Globalization.CultureInfo.InvariantCulture;
    public string CheckedIcon { get; init; } = Icons.Material.Filled.CheckBox;
    public Color Color { get; init; } = default;
    public bool Dense { get; init; } = false;
    public string IndeterminateIcon { get; init; } = Icons.Material.Filled.IndeterminateCheckBox;
    public bool Ripple { get; init; } = true;
    public Size Size { get; init; } = Size.Medium;
    public Color? UncheckedColor { get; init; }
    public string UncheckedIcon { get; init; } = Icons.Material.Filled.CheckBoxOutlineBlank;
    public bool HideText { get; init; } = false;
}