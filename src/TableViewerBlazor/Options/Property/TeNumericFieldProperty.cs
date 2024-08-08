using System.Numerics;

namespace TableViewerBlazor.Options.Property;

public class TeNumericFieldProperty<T> : TeDebouncedInputProperty, ITeNumericFieldProperty
    where T : INumber<T>, IMinMaxValue<T>
{
    /// <summary>
    /// Show clear button.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; } = false;

    /// <summary>
    /// Custom clear icon when <see cref="Clearable"/> is enabled.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public string ClearIcon { get; set; } = Icons.Material.Filled.Clear;

    /// <summary>
    /// Reverts mouse wheel up and down events, if true.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool InvertMouseWheel { get; set; } = false;

    /// <summary>
    /// The minimum value for the input.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public T Min { get; set; } = T.MinValue;

    /// <summary>
    /// The maximum value for the input.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Validation)]
    public T Max { get; set; } = T.MaxValue;

    /// <summary>
    /// The increment added/subtracted by the spin buttons.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public T Step { get; set; } = T.One;

    /// <summary>
    /// Hides the spin buttons, the user can still change value with keyboard arrows and manual update.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public bool HideSpinButtons { get; set; }

    /// <summary>
    ///  Hints at the type of data that might be entered by the user while editing the input.
    ///  Defaults to numeric
    /// </summary>
    [Parameter]
    public override InputMode InputMode { get; set; } = InputMode.numeric;

    /// <summary>
    /// <para>
    /// The pattern attribute, when specified, is a regular expression which the input's value must match in order for the value to pass constraint validation. It must be a valid JavaScript regular expression
    /// Defaults to [0-9,.\-]
    /// To get a numerical keyboard on safari, use the pattern. The default pattern should achieve numerical keyboard.
    /// </para>
    /// <para>Note: this pattern is also used to prevent all input except numbers and allowed characters. So for instance to allow only numbers, no signs and no commas you might change it to [0-9.]</para>
    /// </summary>
    [Parameter]
    public override string? Pattern { get; set; } = @"[0-9,.\-]";

    object ITeNumericFieldProperty.Min => Min;

    object ITeNumericFieldProperty.Max => Max;

    object ITeNumericFieldProperty.Step => Step;
}